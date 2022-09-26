## Fix HTTP Error 502.5- ANCM Out-Of-Process Startup Failure

Hi, I was writing a series of blog posts on how to set up **Continuous Deployment** of an **ABP Framework** application in **Azure Devops** when I encountered the **HTTP Error 502.5- ANCM Out-Of-Process Startup Failure** 

The **Release Pipeline** actually **succeeded** but when navigating to the API endpoint I received **Http Error 502.5 - ANCM Out-Of-Process Startup Failure**

Because there are a few steps to take to overcome this issue, I decided to write a separate blog post about it.

![HTTP Error 502.5 - ANCM Out-Of-Process Startup Failure](../images/ancm_out_of_process_startup_failure.png)

### Find out Error Reason

* Open the **Debug Console** in the **Kudu Engine** in **[YourAppName]api.scm.azurewebsites.net**
* Start **[YourAppName].HttpApi.Host.exe** in the **Debug Console** to find out the error reason
  
  ```bash
    C:\home\site\wwwroot>[YourAppName].HttpApi.Host.exe
  ```

**ERROR: Internal.Cryptography.CryptoThrowHelper+WindowsCryptographicException: Access is denied.**

```bash
Host terminated unexpectedly!
Volo.Abp.AbpInitializationException: An error occurred during ConfigureServicesAsync phase of the module Volo.Abp.OpenIddict.AbpOpenIddictAspNetCoreModule, Volo.Abp.OpenIddict.AspNetCore, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null. See the inner exception for details.
 ---> Internal.Cryptography.CryptoThrowHelper+WindowsCryptographicException: Access is denied.
```

### ABP Support Solution

On the [ABP Support](https://support.abp.io/QA/Questions/3664/Azure-5003-error-Access-Denied) they propose the following solution:

#### Update ABP Framework application

* In the **[YourAppName]HttpApiHostModule** of the **[YourAppName].HttpApi.Host** project add the code below

```csharp

public override void PreConfigureServices(ServiceConfigurationContext context)
{
     var hostingEnvironment = context.Services.GetHostingEnvironment();

     if (!hostingEnvironment.IsDevelopment())
     {
         PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
         {
             options.AddDevelopmentEncryptionAndSigningCertificate = false;
         });

         PreConfigure<OpenIddictServerBuilder>(builder =>
         {
             builder.AddEncryptionCertificate(GetEncryptionCertificate(hostingEnvironment, context.Services.GetConfiguration()));
         });
     }
}

private X509Certificate2 GetEncryptionCertificate(IWebHostEnvironment hostingEnv, IConfiguration configuration)
{
    var fileName = configuration["MyAppCertificate:X590:FileName"]; //*.pfx 
    var passPhrase = configuration["MyAppCertificate:X590:PassPhrase"]; // pass phrase (XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX)
    var file = Path.Combine(hostingEnv.ContentRootPath, fileName);

    if (!File.Exists(file))
    {
        throw new FileNotFoundException($"Signing Certificate couldn't found: {file}");
    }

    return new X509Certificate2(file, passPhrase);
}

```

* In the **appsettings.json** file of the **[YourAppName].HttpApi.Host** project add section below

```bash
"MyAppCertificate": { 
      "X590": 
        { 
          "FileName": "encryption-certificate.pfx", 
          "PassPhrase": "YourPassPhraseHere" 
          }  
    }
```

#### Generate encryption-certificate.pfx file

* In a temp folder on your computer create a **new Console** app

```bash  
    dotnet new console -n OpenIddictSigning
```  

* Replace the content of the **Program.cs** file

```csharp
// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using var algorithm = RSA.Create(keySizeInBits: 2048);

var subject = new X500DistinguishedName("CN=Fabrikam Encryption Certificate");
var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment, critical: true));

var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));

File.WriteAllBytes("encryption-certificate.pfx", certificate.Export(X509ContentType.Pfx, "YourPassPhraseHere"));

Console.WriteLine("encryption-certificate.pfx file generated!");

```

* Run the console app to generate the **encryption-certificate.pfx** file

* Copy/paste the **encryption-certificate.pfx** file into the root of the **[YourAppName].HttpApi.Host** project

* In the **[YourAppName].HttpApi.Host.csproj** file add section below:

  ```bash
  <ItemGroup>
    <None Remove="encryption-certificate.pfx" />
    <Content Include="encryption-certificate.pfx">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
  ```

* Remove the line *.pfx in the .gitignore file of the ABP Framework application

* Open a command prompt in the root folder of your project. Add, Commit and Push all your changes to your GitHub repo to trigger a new build and release.

```bash
    git add .
    git commit -m EncryptionCertificate
    git push
```

After implementing the suggested solution, the deployed API still threw HTTP Error 502.5 - ANCM Out-Of-Process Startup Failure

### Find out Error Reason again

* Open the **Debug Console** in the **Kudu Engine** in **[YourAppName]api.scm.azurewebsites.net**
* Start **[YourAppName].HttpApi.Host.exe** in the **Debug Console** to find out the error reason
  
  ```bash
    C:\home\site\wwwroot>[YourAppName].HttpApi.Host.exe
  ```

**ERROR: System.Security.Cryptography.CryptographicException: System cannot find specified file.**

As you can see, there is still something wrong with finding the certificate.

```bash
Internal.Cryptography.CryptoThrowHelper+WindowsCryptographicException: The system cannot find the file specified.
   at Internal.Cryptography.Pal.CertificatePal.FilterPFXStore(ReadOnlySpan`1 rawData, SafePasswordHandle password, PfxCertStoreFlags pfxCertStoreFlags
```

On **StackOverflow** I found the article [CryptographicException was unhandled: System cannot find the specified file](https://stackoverflow.com/questions/17840825/cryptographicexception-was-unhandled-system-cannot-find-the-specified-file) where I found that the problem was situated in the return statement of the **GetEncryptionCertificate** method from above.

 I updated the return statement and this fixed the problem.

```csharp
// from
return new X509Certificate2(file, passPhrase);

// to
return new X509Certificate2(file, passPhrase, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

```

* Open a command prompt in the root folder of your project. Add, Commit and Push all your changes to your GitHub repo to trigger a new Build and Release to see if your API is running correctly.

```bash
    git add .
    git commit -m CannotFindSpecifiedFile
    git push
```



[[Continue with part 8]](tutorial/../7.deployment-succeeded-web-app-not-working-fix-the-issues.md)