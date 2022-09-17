# ABP Framework to Azure!

## Continuous Deployment of an ABP Framework app to Azure DevOps

A step-by-step tutorial on how to set up Continuous Deployment in Azure DevOps of an ABP Framework application.

## Source Code

The sample application has been developed with **Blazor** as UI framework and **SQL Server** as database provider, but the same principles apply for other UI frameworks/database providers.

The source code of the completed application is [available on GitHub](https://github.com/bartvanhoey/Abp2Azure).

## Requirements

The following tools are needed to be able to run the solution.

* .NET 6.0 SDK
* VsCode, Visual Studio 2022 or another compatible IDE
* ABP CLI version 6.0.0


[Part 1: Create a new GitHub repository](#Create a new GitHub repository)

[Part 2: Create a new ABP Framework application](https://abpioazuredevopsblazor.azurewebsites.net/part2)

### Create a new GitHub repository

[Part 3: Create an SQL Database on Azure and change connection string in appsettings.json files](https://abpioazuredevopsblazor.azurewebsites.net/part3)

[Part 4: Set up the Build pipeline in AzureDevops and publish the Build Artifacts](https://abpioazuredevopsblazor.azurewebsites.net/part4)

[Part 5: Create a Web App in the Azure Portal to deploy [YourAppName].HttpApi.Host project](https://abpioazuredevopsblazor.azurewebsites.net/part5)

[Part 6: Create a Release pipeline in the AzureDevops and deploy [YourAppName].HttpApi.Host project](https://abpioazuredevopsblazor.azurewebsites.net/part6)

[Part 7: Release pipeline finished, Deployment [YourAppName].HttpApi.Host project succeeded, but Web App still not working. How to fix the issues?](https://abpioazuredevopsblazor.azurewebsites.net/part7)

[Part 8: Create a Web App in the Azure Portal to deploy [YourAppName].Blazor project](https://abpioazuredevopsblazor.azurewebsites.net/part8)

[Part 9: Add an extra Stage in the Release pipeline in the AzureDevops to deploy [YourAppName].Blazor project](https://abpioazuredevopsblazor.azurewebsites.net/part9)

[Part 10: Release pipeline finished, Deployment [YourAppName].Blazor project succeeded, but Web App still not working. How to fix the issues?](https://abpioazuredevopsblazor.azurewebsites.net/part10)