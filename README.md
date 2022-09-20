## ABP Framework to Azure

[![Build Status](https://dev.azure.com/AbpIoAzureDevopsOrg/Abp2AzureProj/_apis/build/status/Abp2AzureBuildPipeline?branchName=main)](https://dev.azure.com/AbpIoAzureDevopsOrg/Abp2AzureProj/_build/latest?definitionId=3&branchName=main)

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

[1. Create a new GitHub repository](tutorial/1.create-a-new-github-repository.md)

[2. Create a new ABP Framework application](tutorial/2.create-a-new-abp-framework-application.md)

[3. Create an SQL Database in Azure](tutorial/3.create-an-sql-database-in-azure.md)

[4. Set up the Build pipeline in AzureDevops](tutorial/4.set-up-a-build-pipeline-in-azuredevops.md)

[5. Create a Web App in the Azure Portal](tutorial/5.create-a-web-app-in-the-azure-portal-for-the-api-project.md)

[6. Create Release pipeline and deploy HttpApi.Host project](tutorial/6.create-a-release-pipeline-and-deploy-httpapi-host-project.md)

[7. API Deployment succeeded. Web App not working. Fix the issues!](tutorial/7.deployment-succeeded-web-app-not-working-fix-the-issues.md)

[8. Create a Web App in the Azure Portal for the Blazor project](tutorial/8.create-a-web-app-in-the-azure-portal-for-the-blazor-project.md)

[9. Add a stage in the Release pipeline to deploy the Blazor project](tutorial/9.add-an-extra-stage-in-the-release-pipeline-for-the-blazor-project.md)

[10.Blazor Deployment succeeded. Web App not working. Fix the issues!](tutorial/10.deployment-blazor-project-succeeded-web-app-still-not-working-fix-the-issues.md)












### Create Release pipeline and deploy HttpApi.Host project



### Deployment succeeded, but Web App not working. Fix the issues






      1. When this new Build has finished, a new Release will start. Wait until the Release has finished and the Deployment Succeeded
      2.  Navigate to the URL of the Web App. You probably see the error **An error occurred while starting the application.**
      <Figure Size="FigureSize.None">
        <FigureImage Source="images/an_error_when_starting_the_application.jpg" />
      </Figure>
      1.  Open the **Debug Console** in the **Kudu** window by navigating to [YourAppNameapi].scm.azurewebsites.net
      ```bashhttps://[YourAppName]api.scm.azurewebsites.net</code></pre>
      1.  Try to invoke an **error description** by entering the command below in the **home/site/wwwroot** folder of the **Debug Console**
      ```bashdotnet [YourAppName].HttpApi.Host.dll</code></pre>
      1.  If you receive no Error description. Go to **Program.cs** in the **[YourAppName].HttpApi.Host** project and comment out the **if debug statements**
      <Figure Size="FigureSize.None">
        <FigureImage Source="images/comment_out_if_debug_statements_in_ProgramCs.jpg" />
      </Figure>
      1.  Add, Commit and Push all your changes to your GitHub repository
       ```bash
git add .
git commit -m CommentOutDebugStatements
git push
      </code></pre>
      15. Wait until the new Build and new Release have finished and the Deployment has succeeded
      16. Navigate to the URL of the Web App. You should see the same error **An error occurred while starting the application.** again
       17. Open the **Debug Console** in the **Kudu** window by navigating to [YourAppNameapi].scm.azurewebsites.net
      ```bashhttps://[YourAppName]api.scm.azurewebsites.net</code></pre>
      18. Enter the command below in the wwwroot folder of the **Debug Console** to start the application. Now you should see the detailed error description. The file **tempkey.rsa** is missing
      ```bashdotnet [YourAppName].HttpApi.Host.dll</code></pre>
      <Figure Size="FigureSize.None">
        <FigureImage Source="images/could_not_find_file_tempkey.rsa.jpg" />
      </Figure>
      19. Add the section below to the **[YourAppName].HttpApi.Host.csproj** file to copy the **missing tempkey.rsa** file to the output directory 
      ```bash
&lt;ItemGroup&gt
  &lt;None Update="tempkey.rsa"&gt
    &lt;CopyToOutputDirectory&gtPreserveNewest&lt;/CopyToOutputDirectory&gt
  &lt;/None&gt
&lt;/ItemGroup&gt
      </code></pre>
      20. Add, Commit and Push all your changes to your GitHub repository
      ```bash
git add .
git commit -m CopyToOutputDirectory
git push
      </code></pre>
      21. Wait until the new Build and Release have finished and the Deployment has succeeded
      22. Navigate to the URL of the Web App to see if the error is gone
      23. It's possible that you get another error: **This page isn’t working**
      24. Open the **Debug Console** in the **Kudu** window by navigating to **[YourAppNameapi].scm.azurewebsites.net**
      ```bashhttps://[YourAppName]api.scm.azurewebsites.net</code></pre>
      25. Enter the command below in the **wwwroot** folder of the **Debug Console** to get a more specific error description
      ```bashdotnet [YourAppName].HttpApi.Host.dll</code></pre>
      26. Probably you receive the error description below
      <Figure Size="FigureSize.None">
        <FigureImage Source="images/client_not_allowed_to_access_server.jpg" />
      </Figure>
      27. Go to your **Azure Portal** and select your **[YourAppName]server**
      28. Click on <b>Firewalls and virtual networks</b> in the left menu
      29. Select <b>Yes</b> in the **Allow Azure services and resources to access this server** toggle
      30. Click the <b>Save</b> button. Click <b>OK</b> in the **Successfully updated server firewall rules** window. Close the window 
      31. Navigate to the URL of the Web App and Refresh the page
      32. Your **[YourAppName].HttpApi.Host** project should now <b>be up and running</b> and the **Swagger** page is served by your Web App in Azure
      <Figure Size="FigureSize.None">
        <FigureImage Source="images/swagger_page_served_by_web_app_on_azure.jpg" />
      </Figure>
    </Part>

