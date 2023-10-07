using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Moonglade.ContentSecurity.Moderators;

namespace Moonglade.ContentSecurity;

public static class AzureWordFilter
{
    [FunctionName("AzureMask")]
    public static async Task<IActionResult> Mask(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "azure/mask")]
        Payload req, ILogger log)
    {
        log.LogInformation("C# HTTP trigger function azure/mask processed a request.");

        var oask = Environment.GetEnvironmentVariable("OcpApimSubscriptionKey");
        var cred = new ApiKeyServiceClientCredentials(oask);

        IModerator moderator = new AzureContentModerator(new ContentModeratorClient(cred)
        {
            Endpoint = Environment.GetEnvironmentVariable("Endpoint")
        });

        var result = await moderator.ModerateContent(req.Content);

        var response = new ModeratorResponse
        {
            Moderator = nameof(AzureContentModerator),
            Mode = nameof(Mask),
            OriginAspNetRequestId = req.OriginAspNetRequestId,
            ProcessedContent = result
        };

        return new OkObjectResult(response);
    }

    [FunctionName("AzureDetect")]
    public static async Task<IActionResult> Detect(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "azure/detect")]
        Payload req, ILogger log)
    {
        log.LogInformation("C# HTTP trigger function azure/detect processed a request.");

        var oask = Environment.GetEnvironmentVariable("OcpApimSubscriptionKey");
        var cred = new ApiKeyServiceClientCredentials(oask);

        IModerator moderator = new AzureContentModerator(new ContentModeratorClient(cred)
        {
            Endpoint = Environment.GetEnvironmentVariable("Endpoint")
        });

        var result = await moderator.HasBadWord(req.Content);

        var response = new ModeratorResponse
        {
            Moderator = nameof(AzureContentModerator),
            Mode = nameof(Detect),
            OriginAspNetRequestId = req.OriginAspNetRequestId,
            ProcessedContent = null,
            Positive = result
        };

        return new OkObjectResult(response);
    }
}