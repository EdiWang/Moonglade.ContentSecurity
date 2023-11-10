# Moonglade.ContentSecurity

The Azure Function used by my blog (https://edi.wang) for filtering harmful text in order to live in China.

This Function provides two types of moderation:

- Local: filter harmful text by a local dictionary
- Azure: filter harmful text by Azure Content Moderator

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

## Creating Your Own API

If you have any reason to not use this project, such as a dislike for Azure or C#, it is completely acceptable to develop your own API. Moonglade utilizes standard REST API calls, as long as your API functions with the same request and response interface contract outlined below.

### Request Example

```json
{
    "originAspNetRequestId": "1",
    "contents": [
        {
            "Id": "1",
            "RawText": "fuck some shit"
        },
        {
            "Id": "2",
            "RawText": "suck my dick"
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
            "processedText": "**** some ****"
        },
        {
            "id": "2",
            "processedText": "suck my ****"
        }
    ],
    "positive": null
}
```

Please see source code for more details.

## 免责申明

此项目(Moonglade.ContentSecurity)及其配套组件均为免费开源的产品，仅用于学习交流，并且不直接向中国提供服务，中国用户请于下载后立即删除。

任何中国境内的组织及个人不得使用此项目(Moonglade.ContentSecurity)及其配套组件构建任何形式的面向中国境内用户的网站或服务。

不可用于任何违反中华人民共和国(含台湾省)或使用者所在地区法律法规的用途。

因为作者即本人仅完成代码的开发和开源活动(开源即任何人都可以下载使用)，从未参与用户的任何运营和盈利活动。

且不知晓用户后续将程序源代码用于何种用途，故用户使用过程中所带来的任何法律责任即由用户自己承担。

[《开源软件有漏洞，作者需要负责吗？是的！》](https://go.edi.wang/aka/os251)