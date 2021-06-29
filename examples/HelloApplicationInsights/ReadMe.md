![Essential Logging](../../docs/images/diagnostics-logo-64.png)

# HelloApplicationInsights

## Running the Example

Pre-requisites:

* PowerShell, https://github.com/PowerShell/PowerShell
* Azure CLI, https://docs.microsoft.com/en-us/cli/azure

You need to create an Azure Log Analytics Workspace and linked
Application Insights instance in your default subscription. (You can also
pass in a different subscription if you want.) 

The following scripts will create the needed resources in Azure:

```
az login
$defaultSubscription = (az account show | ConvertFrom-Json).id
./infrastructure/deploy-infrastructure.ps1 $defaultSubscription hello-ai
```

The scripts will output the Instrumentation Key needed for the apps.

By default the scripts will create resources in WestUS in a resource group
named 'hello-ai' (this can be changed in the variables). Azure CLI commands
are (mostly) idempotent, so you can re-run the scripts as needed.

Then run the HelloApplicationInsights example in a console:

```powershell
dotnet run --project ./examples/HelloApplicationInsights
```

Open the Log Analytics Workspace in Azure, and you can use a query similar
to the following:

```
union AppTraces, AppExceptions
| where TimeGenerated > ago(1h)
| where Properties.CategoryName !startswith 'Microsoft.Extensions'
| order by TimeGenerated desc
| project TimeGenerated, Message, SeverityLevel, OperationId, AppRoleInstance, AppVersion, Properties.CategoryName, Properties
```


**Example output: Elasticsearch via Kibana** 

## Configuration

