## ABP Framework goes Azure

[![Build Status](https://dev.azure.com/AbpIoAzureDevopsOrg/Abp2AzureProj/_apis/build/status/Abp2AzureBuildPipeline?branchName=main)](https://dev.azure.com/AbpIoAzureDevopsOrg/Abp2AzureProj/_build/latest?definitionId=3&branchName=main)

### Continuous Deployment (CD) of an ABP Framework application to Azure DevOps

_Continuous deployment_ is a _software engineering approach_ in which _software functionalities are delivered frequently and through automated deployments_. (Wikipedia)

A **step-by-step tutorial** on how to set up **Continuous Deployment** of an **ABP Framework application**.

### Source Code

The sample application has been developed with **Blazor** as UI framework and **SQL Server** as database provider, but the same principles apply for other **UI frameworks like Angular, MVC or database providers like MongoDb, MySql, Progress**, ...

### Requirements

The following tools are needed to be able to run the solution.

* .NET 6.0 SDK
* VsCode, Visual Studio 2022 or another compatible IDE
* ABP CLI version 6.0.0

### Step-by-step

[1. Create a new GitHub repository](tutorial/1.create-a-new-github-repository.md)

[2. Create a new ABP Framework application](tutorial/2.create-a-new-abp-framework-application.md)

[3. Create an SQL Database in Azure](tutorial/3.create-an-sql-database-in-azure.md)

[4. Set up the Build pipeline in AzureDevops](tutorial/4.set-up-a-build-pipeline-in-azuredevops.md)

[5. Create a Web App in the Azure Portal for the API](tutorial/5.create-a-web-app-in-the-azure-portal-for-the-api-project.md)

[6. Create a Release pipeline to deploy the HttpApi.Host project](tutorial/6.create-a-release-pipeline-and-deploy-httpapi-host-project.md)

[7. API Deployment succeeded. Web App not working. Fix the issues!](tutorial/7.deployment-succeeded-web-app-not-working-fix-the-issues.md)

[8. Create a Web App in the Azure Portal for the Blazor project](tutorial/8.create-a-web-app-in-the-azure-portal-for-the-blazor-project.md)

[9. Add a stage in the Release pipeline to deploy the Blazor project](tutorial/9.add-an-extra-stage-in-the-release-pipeline-for-the-blazor-project.md)

[10. Blazor Deployment succeeded. Web App not working. Fix the issues!](tutorial/10.deployment-blazor-project-succeeded-web-app-still-not-working-fix-the-issues.md)
