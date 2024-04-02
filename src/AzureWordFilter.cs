﻿using Azure;
using Azure.AI.ContentSafety;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moonglade.ContentSecurity.Moderators;

namespace Moonglade.ContentSecurity;

public class AzureWordFilter(ILogger<AzureWordFilter> logger)
{
    [Function("AzureMask")]
    public async Task<IActionResult> Mask(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "azure/mask")]
        Payload req)
    {
        logger.LogInformation("C# HTTP trigger function azure/mask processed a request.");

        var moderator = await GetAzureModerator();

        var processedContents = new List<ProcessedContent>();

        foreach (var reqContent in req.Contents)
        {
            var result = await moderator.ModerateContent(reqContent.RawText);
            processedContents.Add(new()
            {
                Id = reqContent.Id,
                ProcessedText = result
            });
        }

        var response = new ModeratorResponse
        {
            Moderator = nameof(AzureAIContentSafetyModerator),
            Mode = nameof(Mask),
            OriginAspNetRequestId = req.OriginAspNetRequestId,
            ProcessedContents = processedContents.ToArray()
        };

        return new OkObjectResult(response);
    }

    [Function("AzureDetect")]
    public async Task<IActionResult> Detect(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "azure/detect")]
        Payload req)
    {
        logger.LogInformation("C# HTTP trigger function azure/detect processed a request.");

        var moderator = await GetAzureModerator();
        var result = await moderator.HasBadWord(req.Contents.Select(p => p.RawText).ToArray());

        var response = new ModeratorResponse
        {
            Moderator = nameof(AzureAIContentSafetyModerator),
            Mode = nameof(Detect),
            OriginAspNetRequestId = req.OriginAspNetRequestId,
            ProcessedContents = null,
            Positive = result
        };

        return new OkObjectResult(response);
    }

    private static async Task<IModerator> GetAzureModerator()
    {
        var oask = Environment.GetEnvironmentVariable("OcpApimSubscriptionKey");
        var cred = new AzureKeyCredential(oask!);

        IModerator moderator = new AzureAIContentSafetyModerator(new(new(Environment.GetEnvironmentVariable("Endpoint")!), cred));

        BlocklistClient blocklistClient = new BlocklistClient(new(Environment.GetEnvironmentVariable("Endpoint")!), cred);

        var blocklistName = "Moonglade.ContentSecurity.BlockList";
        var blocklistDescription = "User defined keywords";

        var data = new
        {
            description = blocklistDescription,
        };

        var createResponse = await blocklistClient.CreateOrUpdateTextBlocklistAsync(blocklistName, RequestContent.Create(data));
        if (createResponse.Status is 200 or 201)
        {
            var keywordsList = Environment.GetEnvironmentVariable("Keywords")!.Split('|').ToList();
            var blockItems = keywordsList.Select(p => new TextBlocklistItem(p)).ToArray();

            var addedBlockItems = await blocklistClient.AddOrUpdateBlocklistItemsAsync(blocklistName,
                new AddOrUpdateTextBlocklistItemsOptions(blockItems));

            if (addedBlockItems is { Value: not null })
            {
                return moderator;
            }
        }

        return null;
    }
}