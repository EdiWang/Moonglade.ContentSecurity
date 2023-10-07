# Moonglade.ContentSecurity

The Azure Function used by my blog (https://edi.wang) for filtering harmful text in order to live in China.

This Function provides two types of moderation:

- Local: filter harmful text by a local dictionary
- Azure: filter harmful text by Azure Content Moderator

**NOTE: This project will be upgraded to .NET 8.0 LTS in November 2023, please prepare your server runtime in advance.**

## Get Started

Tools | Alternative
--- | ---
[.NET 6.0 SDK](http://dot.net) | N/A
[Visual Studio 2022](https://visualstudio.microsoft.com/) with Azure Development payload| [Visual Studio Code](https://code.visualstudio.com/)
[Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/) | N/A
[Azure CLI](https://docs.microsoft.com/en-us/cli/azure/?view=azure-cli-latest) | N/A

## Deployment

Build and deploy the project to your Azure subscription.

Recommendations:

- Enable 64 bit worker process in Azure Function App settings.
- Enable Always On in Azure Function App settings.
- Enable Application Insights in Azure Function App settings.
- Do NOT use consumption plan due to cold start time.

### Local Moderation

Once deployed to Azure, set the following environment variables in Azure Portal (Configuration blade) or Azure CLI:

- `Keywords`: the keywords to be filtered, separated by "|"

### Azure Moderation

Pre-requisite: create an Azure Content Moderator resource in Azure Portal, and get the endpoint and subscription key.

Once deployed to Azure, set the following environment variables in Azure Portal (Configuration blade) or Azure CLI:

- `Endpoint`: the endpoint of your Azure Content Moderator
- `OcpApimSubscriptionKey`: the subscription key of your Azure Content Moderator

## Development and Debugging

For development, create ```local.settings.json``` under "**./src/**", this file defines development time settings. It is by default ignored by git, so you will need to manange it on your own.

Sample ```local.settings.json``` file

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "Keywords": "fuck|shit|dick",
    "Endpoint": "https://**********.cognitiveservices.azure.com/",
    "OcpApimSubscriptionKey": "**********"
  }
}
```
