using Azure.Identity;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.AppService.Models;
using System.Diagnostics;

        // Authenticate with Azure using DefaultAzureCredential
        ArmClient armClient = new ArmClient(new DefaultAzureCredential());
        SubscriptionResource subscription = await armClient.GetDefaultSubscriptionAsync();

        // Define the App Service Plan
        string appServicePlanName = "myAppServicePlanluiscoco1974";
        string resourceGroupName = "myRg";
        string webAppName = "myWebAppluiscoco1974";

        Console.WriteLine("Checking for existing Resource Group...");

        // Check if the Resource Group exists, create if it does not exist
       
        bool resourceGroupExists = await subscription.GetResourceGroups().ExistsAsync(resourceGroupName);
        if (!resourceGroupExists)
        {
            Console.WriteLine($"Resource Group '{resourceGroupName}' not found. Creating new Resource Group...");

            var resourceGroupData = new ResourceGroupData(AzureLocation.WestUS);
            await subscription.GetResourceGroups().CreateOrUpdateAsync(WaitUntil.Completed, resourceGroupName, resourceGroupData);

            Console.WriteLine($"Resource Group '{resourceGroupName}' created successfully.");
        }
        else
        {
            await subscription.GetResourceGroups().GetAsync(resourceGroupName);
            Console.WriteLine($"Resource Group '{resourceGroupName}' already exists.");
        }

        Console.WriteLine("Creating App Service Plan...");

        var appServicePlanData = new AppServicePlanData(AzureLocation.WestUS)
        {
            Sku = new AppServiceSkuDescription
            {
                Name = "B1", // Standard tier
                Tier = "Basic",
                Size = "B1",
                Family = "B",
                Capacity = 1
            }
        };

        var appServicePlan = await resourceGroup.GetAppServicePlans().CreateOrUpdateAsync(WaitUntil.Completed, appServicePlanName, appServicePlanData);

        Console.WriteLine($"App Service Plan '{appServicePlanName}' created successfully.");

        var webAppData = new WebSiteData(AzureLocation.WestUS)
        {
            AppServicePlanId = appServicePlan.Value.Id
        };

        var webApp = await resourceGroup.GetWebSites().CreateOrUpdateAsync(WaitUntil.Completed, webAppName, webAppData);

        Console.WriteLine($"Web App '{webAppName}' created successfully.");