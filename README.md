# How to create Azure AppServicePlan and Azure WebApp with Azure SDK for .NET

## 1. Create the Azure Authorization credentials

```csharp
using Azure.Identity;
...
// Authenticate with Azure using DefaultAzureCredential
ArmClient armClient = new ArmClient(new DefaultAzureCredential());
SubscriptionResource subscription = await armClient.GetDefaultSubscriptionAsync();
...
```


## 2. Create the Azure ServicePlan




## 3. Create the Azure WebApp


## 4. Whole application source code

```csharp
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

Console.WriteLine("Creating App Service Plan...");

var resourceGroup = await subscription.GetResourceGroups().GetAsync(resourceGroupName);
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

var appServicePlan = await resourceGroup.Value.GetAppServicePlans().CreateOrUpdateAsync(WaitUntil.Completed, appServicePlanName, appServicePlanData);

Console.WriteLine($"App Service Plan '{appServicePlanName}' created successfully.");

var webAppData = new WebSiteData(AzureLocation.WestUS)
{
    AppServicePlanId = appServicePlan.Value.Id
};

var webApp = await resourceGroup.Value.GetWebSites().CreateOrUpdateAsync(WaitUntil.Completed, webAppName, webAppData);

Console.WriteLine($"Web App '{webAppName}' created successfully.");
```




