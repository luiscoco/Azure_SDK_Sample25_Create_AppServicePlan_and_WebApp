using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.AppService.Models;

// Authenticate with Azure using DefaultAzureCredential
ArmClient armClient = new ArmClient(new DefaultAzureCredential());
SubscriptionResource subscription = await armClient.GetDefaultSubscriptionAsync();

// Define the App Service Plan
string appServicePlanName = "myAppServicePlanluiscocoenriquez1974";
string resourceGroupName = "myRg";

Console.WriteLine("Creating App Service Plan...");

var resourceGroup = await subscription.GetResourceGroups().GetAsync(resourceGroupName);
var appServicePlanData = new AppServicePlanData(AzureLocation.EastUS)
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

var appServicePlan = await resourceGroup.Value.GetAppServicePlans().CreateOrUpdateAsync(WaitUntil.Completed, appServicePlanName, appServicePlanData);

Console.WriteLine($"App Service Plan '{appServicePlanName}' created successfully.");