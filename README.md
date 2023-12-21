# How to create Azure App Service Plan and Azure Web App with Azure SDK for .NET

This sample source code is stored in repo: https://github.com/luiscoco/Azure_SDK_Sample25_Create_AppServicePlan_and_WebApp/tree/master

## 1. Create the Azure Authorization credentials

This code is initializing a client for Azure Resource Manager with default credentials and then asynchronously fetching details about the default Azure subscription associated with those credentials.

```csharp
using Azure.Identity;
...
ArmClient armClient = new ArmClient(new DefaultAzureCredential());
SubscriptionResource subscription = await armClient.GetDefaultSubscriptionAsync();
...
```

## 2. Create the Azure ServicePlan

An instance of AppServicePlanData is created, which is a class used to define the properties of an Azure App Service Plan. 

This instance is assigned to appServicePlanData.

AzureLocation.WestUS specifies the location for the App Service Plan, which in this case is West US.

The Sku property of the AppServicePlanData object is set to a new instance of AppServiceSkuDescription. This includes various parameters such as:

**Name**: "B1", indicating the specific SKU name.

**Tier**: "Basic", indicating the pricing tier.

**Size**: "B1", indicating the size of the plan.

**Family**: "B", indicating the family of the plan.

**Capacity**: 1, indicating the number of instances.

**IMPORTANT NOTE**: if we would like to specify in App Service Plan more parameters **AppServicePlanData** and **AppServiceSkuDescription**

```csharp
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
```

## 3. Create the Azure WebApp

This code is configuring and deploying a web application in an Azure environment, specifying its location and service plan, and then either creating a new application or updating an existing one in a specific resource group.

**IMPORTANT NOTE**: if we would like to specify in App Service Plan more parameters **WebSiteData**

```csharp
...
var webAppData = new WebSiteData(AzureLocation.WestUS)
{
    AppServicePlanId = appServicePlan.Value.Id
};

var webApp = await resourceGroup.GetWebSites().CreateOrUpdateAsync(WaitUntil.Completed, webAppName, webAppData);

Console.WriteLine($"Web App '{webAppName}' created successfully.");
```

## 4. Whole application source code for creating Azure App Service Plan and Azure Web App

```csharp
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
```

## 5. In Azure Portal verify the new services

![image](https://github.com/luiscoco/Azure_SDK_Sample25_Create_AppServicePlan_and_WebApp/assets/32194879/aad6b8b6-ad69-44ab-b358-20f0ccae7070)




