## ABP Framework to Azure!

### Continuous Deployment of an ABP Framework app to Azure DevOps

A step-by-step tutorial on how to set up Continuous Deployment in Azure DevOps of an ABP Framework application.

### Source Code

The sample application has been developed with **Blazor** as UI framework and **SQL Server** as database provider, but the same principles apply for other UI frameworks/database providers.

The source code of the completed application is [available on GitHub](https://github.com/bartvanhoey/Abp2Azure).

### Requirements

The following tools are needed to be able to run the solution.

* .NET 6.0 SDK
* VsCode, Visual Studio 2022 or another compatible IDE
* ABP CLI version 6.0.0

### Step-by-step Tutorial

[1. Create a new GitHub repository](#create-a-new-github-repository)

[2. Create a new ABP Framework application](#create-a-new-abp-framework-application)

[3. Create an SQL Database in Azure](#create-an-sql-database-in-azure)

[4. Set up the Build pipeline in AzureDevops](#set-up-the-build-pipeline-in-azuredevops)

[5. Create a Web App in the Azure Portal](#create-a-web-app-in-the-azure-portal)

[6. Create Release pipeline and deploy HttpApi.Host project](#create-release-pipeline-and-deploy-httpapihost-project)

[Part 7: Release pipeline finished, Deployment [YourAppName].HttpApi.Host project succeeded, but Web App still not working. How to fix the issues?](https://abpioazuredevopsblazor.azurewebsites.net/part7)

[Part 8: Create a Web App in the Azure Portal to deploy [YourAppName].Blazor project](https://abpioazuredevopsblazor.azurewebsites.net/part8)

[Part 9: Add an extra Stage in the Release pipeline in the AzureDevops to deploy [YourAppName].Blazor project](https://abpioazuredevopsblazor.azurewebsites.net/part9)

[Part 10: Release pipeline finished, Deployment [YourAppName].Blazor project succeeded, but Web App still not working. How to fix the issues?](https://abpioazuredevopsblazor.azurewebsites.net/part10)

### Create a new GitHub repository

Go to [GitHub](https://github.com) and create a new Repository. Open a command prompt and clone the repository into a folder on your computer.

```bash
    git clone https://github.com/your-username/your-repository-name.git
```

### Create a new ABP Framework application

#### Check your dotnet version. Should be at least 6.0.x

```bash
    dotnet --version
```

#### Install or update the ABP CLI

```bash
    dotnet tool install -g Volo.Abp.Cli || dotnet tool update -g Volo.Abp.Cli
```

#### Create ABP Framework application

Open a command prompt in the repository folder and create a new ABP Framework application.

```bash
    abp new YourAppName -u blazor
```

#### Apply migrations

Open a command prompt in the [YourAppName].DbMigrator project and apply the database migrations

```bash
    dotnet run
```

#### Run API and Blazor projects

Open a command prompt in the [YourAppName].HttpApi.Host project to run the API project. You should get the **Swagger** window.

```bash
    dotnet run
```

Open a command prompt in the [YourAppName].Blazor folder and enter the command below to run the Blazor project. You should get the **ABP Framework Welcome** window.

```bash
    dotnet run
```

Stop both the API and the Blazor project by pressing **CTRL+C**.

#### Commit and push everything to GitHub

Open a command prompt in the root folder of your ABP Framework project and add, commit and push all your changes to your GitHub repository

```bash
git add .
git commit -m your_commit_message_here
git push
```

### Create an SQL Database in Azure

* Login into [Azure Portal](https://portal.azure.com/)

* Click **Create a resource**

* Search for *SQL Database*

* Click the **Create** button in the *SQL Database window*

* Create a new resource group. Name it *rg[YourAppName]*

* Enter *[YourAppName]Db* as database name

* Create a new Server and name it *[YourAppName]server*

* Authentication method: Use Sql authentication

* Enter a [serveradmin] login and passwords. Click the **OK** button

* Click **Configure database**. Go to the *Basic* version and click the **Apply** button

* Click the **Review + create** button. Click **Create**

* Go to Azure Resources and navigate to the **SQL server** when the SQL Database is created

* Click **Networking** under Security left side menu.

* In the Public Access tab, select **Selected networks** and click **Add your client IPv4 address** at the Firewall rules. Save.

* In the **Exceptions** section, select **Allow Azure and resources to access this server** and save

* Go to your **SQL database**, click **Connection strings** and copy the connection string

* Replace the Default connection string in the appsettings.json files of the [YourAppName].HttpApi.Host and the [YourAppName].DbMigrator project

* Do not forget to replace {your_password} with the correct server password you entered in Azure SQL Database

* Open a terminal in the [YourAppName].DbMigrator project and run the command below to apply the db migrations

```bash
    dotnet run
```

* Open a command prompt in the [YourAppName].HttpApi.Host project and start your API.

```bash
    dotnet run
```

Stop the [YourAppName].HttpApi.Host by entering **CTRL+C**

* Open a terminal in the root folder of your project. Add, commit and push all your changes to your GitHub repo

```bash
    git add .
    git commit -m database_created
    git push
```

### Set up the Build pipeline in AzureDevops

* Open the [AzureDevops](https://azure.microsoft.com/en-us/services/devops/) page and click on the **Sign in to Azure Devops link**
* Click on **New organization** and follow the steps to create a new organization. Name it [YourAppName]org
* Enter [YourAppName]Proj as project name in the **Create a project to get started** window
* Select **Public visibility** and click the **Create project** button
* Click on the **Pipelines** button to continue
* Click on the **Create Pipeline** button
* Select **GitHub** in the **Where is your code window?**

![Where is your code window?](/images/where_is_your_code.png)

* Select your GitHub [YourAppName]repo.
* Click on **Approve and install** in the **Repository access** section in the Azure Pipelines window
  
  ![Repository access?](/images/repository_access_section_in_azure_pipelines_window.png)

* You get redirected to the **Configure your pipeline** window. Select **ASP.NET Core (.NET Framework)**
* You get redirected to the **Review your pipeline YAML** window. Click **Save and run**

![Review your pipeline YAML?](/images/review_your_pipeline_yaml.png)

* Click **Save and run** in (commit directly to the main branch checked) the **Save and run** window
* The pipeline should start running.

**ATTENTION:**

Probably the BUILD will fails with this error message. You can read more about [No hosted parallelism has been purchased or granted](https://stackoverflow.com/questions/68405027/how-to-resolve-no-hosted-parallelism-has-been-purchased-or-granted-in-free-tie) on StackOverflow

```bash
    1 error(s), 0 warning(s)
    No hosted parallelism has been purchased or granted. To request a free parallelism grant, please fill out the following form https://aka.ms/azpipelines-parallelism-request
```

* Comment out the VSTest@2 task as this tasks throws an error
* Add the 3 tasks (Run unit tests, Dotnet publish, Publish artifact) below in **azure-pipelines.yml** file.

```bash
trigger:
- main

pool:
  vmImage: 'windows-latest'

variables:
  solution: '**/*.sln'
  buildPlatform: 'Any CPU'
  buildConfiguration: 'Release'

steps:
- task: NuGetToolInstaller@1
  displayName: Install Nuget tool

- task: NuGetCommand@2
  displayName: Restore Nuget packages
  inputs:
    restoreSolution: '$(solution)'

- task: VSBuild@1
  displayName: Build project
  inputs:
    solution: '$(solution)'
    msbuildArgs: '/p:DeployOnBuild=true /p:WebPublishMethod=Package /p:PackageAsSingleFile=true /p:SkipInvalidConfigurations=true /p:DesktopBuildPackageLocation="$(build.artifactStagingDirectory)\WebApp.zip" /p:DeployIisAppPath="Default Web Site"'
    platform: '$(buildPlatform)'
    configuration: '$(buildConfiguration)'
 
# The task belows throws an error: Testhost process exited with error: 
# Cannot use file stream for [...\bin\Debug\net6.0\testhost.deps.json]: No such file or directory
# - task: VSTest@2
#   inputs:
#     platform: '$(buildPlatform)'
#     configuration: '$(buildConfiguration)'
#     batchingBasedOnAgentsOption

- task: DotNetCoreCLI@2
  displayName: Run unit tests
  inputs:
    command: test
    projects: '**/*[Tt]ests/*.csproj'
    arguments: '--configuration $(BuildConfiguration)'

- task: DotNetCoreCLI@2
  displayName: Dotnet publish
  inputs:
    command: publish
    publishWebProjects: True
    arguments: '--configuration $(BuildConfiguration) --output $(build.artifactstagingdirectory)'
    zipAfterPublish: True

- task: PublishBuildArtifacts@1
  displayName: Publish artifact
  inputs:
    PathtoPublish: '$(build.artifactstagingdirectory)'
  condition: succeededOrFailed()
```

* Click **Save**, **Save** and **Run** to start the build pipeline.
* After a successful build, you can see in the **Summary** window **1 published** to the **drop** folder

![Summary after a successful build](/images/summary_after_a_successful_build.png)

### Create a Web App in the Azure Portal

* Login in to the [Azure Portal](https://portal.azure.com/)
* Click on **Create a resource**
* Search for **Web App** in the **Search services and marketplace** field
* Click the **Create** link in the **Web App** icon
* Select rg[YourAppName] in the **Resource Group** dropdown
* Enter [YourAppName]API in the **Name** input field
* Select **.NET 6 (LTS)** in the **Runtime stack** dropdown
* Select **Windows** as **Operating System**
* Select the same **Region** as in the **SQL server** you created in **Part 3**
* Click on **Create new** in the **Windows Plan**. Name it [YourAppName]WinPlan
* Click **Change size** in **Sku and size**. Go for the **Dev/Test Free F1 version** and click the **Apply** button
* Click the **Review + create** button. Click **Create** button
* Click on **Go to resource** when the **Web App** has been created
* Click on **Configuration** in the left menu and click **+ New application setting** in the **Application settings** tab
* Enter **ASPNETCORE_ENVIRONMENT** in the Name field and **Staging** in the Value field. Click the **OK**, **Save and Continue** buttons
* Copy the **URL** in the **Overview** menu of the **Web App** (https://[YourAppName]api.azurewebsites.net)

### Create Release pipeline and deploy HttpApi.Host project

* Open the [AzureDevops](https://azure.microsoft.com/en-us/services/devops/) page and click on the **Sign in to Azure Devops link**
* Click on [YourAppName]Proj and click on **Releases** in the **Pipelines** menu
* Click on the **New pipeline** button in the **No release pipelines found** window
* Select **Azure App Service deployment** and click the **Apply** button
* Enter **[YourAppName]staging** in the **Stage name** field in the **Stage** window. Close window
* Click **+ Add an artifact** in the **Pipeline** tab
* Select the **Build** icon as **Source type** in the **Add an artifact** window
* Select Build pipeline in the **Source (build pipeline)** dropdown and click the **Add** button
* Click on the **Continuous deployment trigger (thunderbolt icon)**
* Set the toggle to **Enabled** in the the **Continuous deployment trigger** window
* Click **+ Add** in **No filters added**. Select **Include** in the **Type** dropdown. Select your branch in the **Build** branch dropdown and close the window
* Click on **the little red circle with the exclamation mark** in the **Tasks** tab menu
* Select your subscription in the **Azure subscription** dropdown
* Click **Authorize** and enter your credentials in the next screens
* After Authorization, select the **[YourAppName]API** in the **App service name** dropdown
* Click on the **Deploy Azure App Service** task
* Select **[YourAppName].HttpApi.Host.zip** in the **Package or folder** input field

  ![Deploy Azure App Service](images/deploy_azure_app_service.png)

* Click on the **Save** icon in the top menu and click **OK**
* Click **Create release** in the top menu. Click **Create** to create a release
* Click on the **Pipeline** tab and wait until the Deployment succeeds
* Navigate to the **URL** (https://[YourAppName]api.azurewebsites.net) of your Web App
* You probably see error **HTTP Error 500.30 - ASP.NET Core app failed to start**

![HTTP Error 500.30 - ASP.NET Core app failed to start](images/http_error_500_30_aspnet_core_failed_to_start.png)



The Deployment succeeded, but the Web App is still having issues. We will fix them in the next part