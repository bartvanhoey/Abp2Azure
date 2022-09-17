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

[Part 4: Set up the Build pipeline in AzureDevops and publish the Build Artifacts](https://abpioazuredevopsblazor.azurewebsites.net/part4)

[Part 5: Create a Web App in the Azure Portal to deploy [YourAppName].HttpApi.Host project](https://abpioazuredevopsblazor.azurewebsites.net/part5)

[Part 6: Create a Release pipeline in the AzureDevops and deploy [YourAppName].HttpApi.Host project](https://abpioazuredevopsblazor.azurewebsites.net/part6)

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

Open the command prompt in the [YourAppName].DbMigrator project again and enter the command below to apply the database migrations

```bash
    dotnet run
```

Open the command prompt in the [YourAppName].HttpApi.Host project and enter the command below to check your API is working

```bash
    dotnet run
```

Stop the [YourAppName].HttpApi.Host by entering CTRL+C

Open the command prompt in the root folder of your project and add, commit and push all your changes to your GitHub repository

```bash
    git add .
    git commit -m database_created
    git push
```
