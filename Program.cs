using Azure.Identity;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.AppService.Models;

ArmClient armClient = new ArmClient(new DefaultAzureCredential());
SubscriptionResource subscription = await armClient.GetDefaultSubscriptionAsync();

string rgName = "myRg123456";
string appServicePlanName = "myAppServicePlanluiscoco123456";
string webAppName = "myWebAppluiscoco123456";

AzureLocation location = AzureLocation.WestEurope;
ArmOperation<ResourceGroupResource> operation = await subscription.GetResourceGroups().CreateOrUpdateAsync(WaitUntil.Completed, rgName, new ResourceGroupData(location));
ResourceGroupResource resourceGroup = operation.Value;
Console.WriteLine(resourceGroup.Data.Name);

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