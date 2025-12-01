# Moonglade.ContentSecurity

The Azure Function used by my blog (https://edi.wang) for filtering harmful text in order to live in China.

This Function filter harmful text by [Azure AI Content Safety](https://learn.microsoft.com/en-us/azure/ai-services/content-safety/?WT.mc_id=AZ-MVP-5002809)

## Get Started

Tools | Alternative
--- | ---
[.NET 10.0 SDK](http://dot.net) | N/A
[Visual Studio 2026](https://visualstudio.microsoft.com/) with Azure Development payload| [Visual Studio Code](https://code.visualstudio.com/)
[Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/) | N/A
[Azure CLI](https://docs.microsoft.com/en-us/cli/azure/?view=azure-cli-latest) | N/A

## Deployment

> IMPORTANT: Please use [release](https://github.com/EdiWang/Moonglade.ContentSecurity/tree/release) branch for deployment!!!

Build and deploy the project to your Azure subscription.

Recommendations:

- Enable 64 bit worker process in Azure Function App settings.
- Enable Always On in Azure Function App settings.
- Enable Application Insights in Azure Function App settings.
- Use App Service Plan hosting model for better performance.

### Azure Moderation

Pre-requisite: create an [Azure AI Content Safety](https://learn.microsoft.com/en-us/azure/ai-services/content-safety/?WT.mc_id=AZ-MVP-5002809) resource in Azure Portal, and get the endpoint and key.

Once deployed to Azure, set the following environment variables in Azure Portal (Configuration blade) or Azure CLI:

- `Endpoint`: the endpoint of your [Azure AI Content Safety](https://learn.microsoft.com/en-us/azure/ai-services/content-safety/?WT.mc_id=AZ-MVP-5002809) resource
- `OcpApimSubscriptionKey`: the key of your [Azure AI Content Safety](https://learn.microsoft.com/en-us/azure/ai-services/content-safety/?WT.mc_id=AZ-MVP-5002809) resource

## Development and Debugging

For development, create ```local.settings.json``` under "**./src/**", this file defines development time settings. It is by default ignored by git, so you will need to manange it on your own.

Sample ```local.settings.json``` file

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "Keywords": "example|harmful|words",
    "Endpoint": "https://<your resource name>.cognitiveservices.azure.com/",
    "OcpApimSubscriptionKey": "<your key>"
  }
}
```
