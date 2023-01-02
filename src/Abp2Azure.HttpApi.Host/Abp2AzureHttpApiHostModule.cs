using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Abp2Azure.EntityFrameworkCore;
using Abp2Azure.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.OpenIddict;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Abp2Azure;

[DependsOn(
    typeof(Abp2AzureHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(Abp2AzureApplicationModule),
    typeof(Abp2AzureEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class Abp2AzureHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("Abp2Azure");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });

        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });
            PreConfigure<OpenIddictServerBuilder>(builder =>
            {
                // In production, it is recommended to use two RSA certificates, 
                // one for encryption, one for signing.
                builder.AddEncryptionCertificate(GetEncryptionCertificate(hostingEnvironment, context.Services.GetConfiguration()));
                builder.AddSigningCertificate(GetSigningCertificate(hostingEnvironment, context.Services.GetConfiguration()));
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
    
        Debug.WriteLine($"{file} - {passPhrase}");

        var encryptionCertificate = new X509Certificate2(file, passPhrase, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
        return encryptionCertificate;
    }
    
    private X509Certificate2 GetSigningCertificate(IWebHostEnvironment hostingEnv,
                            IConfiguration configuration)
{
    var fileName = $"cert-signing.pfx";
    var passPhrase = configuration["MyAppCertificate:X590:PassPhrase"]; 
    var file = Path.Combine(hostingEnv.ContentRootPath, fileName);        
    // if (File.Exists(file))
    // {
    //     var created = File.GetCreationTime(file);
    //     var days = (DateTime.Now - created).TotalDays;
    //     if (days > 180)          
    //         File.Delete(file);
    //     else
    //         return new X509Certificate2(file, passPhrase,
    //                      X509KeyStorageFlags.MachineKeySet);
    // }
    // file doesn't exist or was deleted because it expired
    using var algorithm = RSA.Create(keySizeInBits: 2048);
    var subject = new X500DistinguishedName("CN=Fabrikam Signing Certificate");
    var request = new CertificateRequest(subject, algorithm, 
                        HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    request.CertificateExtensions.Add(new X509KeyUsageExtension(
                        X509KeyUsageFlags.DigitalSignature, critical: true));
    var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
    File.WriteAllBytes(file, certificate.Export(X509ContentType.Pfx, string.Empty));
    return new X509Certificate2(file, passPhrase, X509KeyStorageFlags.MachineKeySet);
    
    var encryptionCertificate = new X509Certificate2(file, passPhrase, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
    return encryptionCertificate;
}
// private X509Certificate2 GetEncryptionCertificate(IWebHostEnvironment hostingEnv,
//                              IConfiguration configuration)
// {
//     var fileName = $"cert-encryption.pfx";
//     var passPhrase = configuration["MyAppCertificate:X590:PassPhrase"]; 
//     var file = Path.Combine(hostingEnv.ContentRootPath, fileName);
//     // if (File.Exists(file))
//     // {
//     //     var created = File.GetCreationTime(file);
//     //     var days = (DateTime.Now - created).TotalDays;
//     //     if (days > 180)
//     //         File.Delete(file);
//     //     else
//     //         return new X509Certificate2(file, passPhrase, 
//     //                         X509KeyStorageFlags.MachineKeySet);
//     // }
//     // file doesn't exist or was deleted because it expired
//     using var algorithm = RSA.Create(keySizeInBits: 2048);
//     var subject = new X500DistinguishedName("CN=Fabrikam Encryption Certificate");
//     var request = new CertificateRequest(subject, algorithm, 
//                         HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
//     request.CertificateExtensions.Add(new X509KeyUsageExtension(
//                         X509KeyUsageFlags.KeyEncipherment, critical: true));
//     var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow,
//                         DateTimeOffset.UtcNow.AddYears(2));
//     File.WriteAllBytes(file, certificate.Export(X509ContentType.Pfx, string.Empty));
//     var encryptionCertificate = new X509Certificate2(file, passPhrase, X509KeyStorageFlags.MachineKeySet);
//     return encryptionCertificate;
// }
    

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureAuthentication(context);
        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureLocalization();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"].Split(','));

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<Abp2AzureDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Abp2Azure.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<Abp2AzureDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Abp2Azure.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<Abp2AzureApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Abp2Azure.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<Abp2AzureApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Abp2Azure.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(Abp2AzureApplicationModule).Assembly);
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"],
            new Dictionary<string, string>
            {
                    {"Abp2Azure", "Abp2Azure API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Abp2Azure API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
            options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
            options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
            options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
            options.Languages.Add(new LanguageInfo("is", "is", "Icelandic", "is"));
            options.Languages.Add(new LanguageInfo("it", "it", "Italiano", "it"));
            options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
            options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
            options.Languages.Add(new LanguageInfo("ro-RO", "ro-RO", "Română"));
            options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
            options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
            options.Languages.Add(new LanguageInfo("es", "es", "Español", "es"));
            options.Languages.Add(new LanguageInfo("el", "el", "Ελληνικά"));
        });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
               {
                   builder
                       .WithOrigins(
                           configuration["App:CorsOrigins"]
                               .Split(",", StringSplitOptions.RemoveEmptyEntries)
                               .Select(o => o.RemovePostFix("/"))
                               .ToArray()
                       )
                       .WithAbpExposedHeaders()
                       .SetIsOriginAllowedToAllowWildcardSubdomains()
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
               });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        // if (!env.IsDevelopment())
        // {
        //     app.UseErrorPage();
        // }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Abp2Azure API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            c.OAuthScopes("Abp2Azure");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
