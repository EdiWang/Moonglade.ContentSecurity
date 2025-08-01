# Moonglade.ContentSecurity

The Azure Function used by my blog (https://edi.wang) for filtering harmful text in order to live in China.

This Function provides two types of moderation:

- Local: filter harmful text by a local dictionary
- Azure: filter harmful text by [Azure AI Content Safety](https://learn.microsoft.com/en-us/azure/ai-services/content-safety/?WT.mc_id=AZ-MVP-5002809)

## Get Started

Tools | Alternative
--- | ---
[.NET 8.0 SDK](http://dot.net) | N/A
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

## Creating Your Own API

If you have any reason to not use this project, such as a dislike for Azure or C#, it is completely acceptable to develop your own API. Moonglade utilizes standard REST API calls, as long as your API functions with the same request and response interface contract outlined below.

### Request Example

```json
{
    "originAspNetRequestId": "1",
    "contents": [
        {
            "Id": "1",
            "RawText": "This is an example of harmful words"
        },
        {
            "Id": "2",
            "RawText": "Glad to meet you"
        }
    ]
}
```

### Response Example

```json
{
    "originAspNetRequestId": "1",
    "moderator": "LocalModerator",
    "mode": "Mask",
    "processedContents": [
        {
            "id": "1",
            "processedText": "This is an * of * *"
        },
        {
            "id": "2",
            "processedText": "Glad to meet you"
        }
    ],
    "positive": null
}
```

Please see source code for more details.
