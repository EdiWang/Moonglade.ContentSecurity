﻿using Microsoft.AspNetCore.Mvc;
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

        var moderator = GetAzureModerator();

        var processedContents = new List<ProcessedContent>();

        foreach (var reqContent in req.Contents)
        {
            var result = await moderator.ModerateContent(reqContent.RawText);
            processedContents.Add(new ProcessedContent
            {
                Id = reqContent.Id,
                ProcessedText = result
            });
        }

        var response = new ModeratorResponse
        {
            Moderator = nameof(AzureContentModerator),
            Mode = nameof(Mask),
            OriginAspNetRequestId = req.OriginAspNetRequestId,
            ProcessedContents = processedContents.ToArray()
        };

        return new OkObjectResult(response);
    }

    [FunctionName("AzureDetect")]
    public static async Task<IActionResult> Detect(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "azure/detect")]
        Payload req, ILogger log)
    {
        log.LogInformation("C# HTTP trigger function azure/detect processed a request.");

        var moderator = GetAzureModerator();
        var result = await moderator.HasBadWord(req.Contents.Select(p => p.RawText).ToArray());

        var response = new ModeratorResponse
        {
            Moderator = nameof(AzureContentModerator),
            Mode = nameof(Detect),
            OriginAspNetRequestId = req.OriginAspNetRequestId,
            ProcessedContents = null,
            Positive = result
        };

        return new OkObjectResult(response);
    }

    private static IModerator GetAzureModerator()
    {
        var oask = Environment.GetEnvironmentVariable("OcpApimSubscriptionKey");
        var cred = new ApiKeyServiceClientCredentials(oask);

        IModerator moderator = new AzureContentModerator(new ContentModeratorClient(cred)
        {
            Endpoint = Environment.GetEnvironmentVariable("Endpoint")
        });
        return moderator;
    }
}