using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Moonglade.ContentSecurity.Moderators;

namespace Moonglade.ContentSecurity;

public static class LocalWordFilter
{
    [FunctionName("LocalMask")]
    public static async Task<IActionResult> Mask(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "local/mask")] Payload req, ILogger log)
    {
        log.LogInformation("C# HTTP trigger function local/mask processed a request.");

        var moderator = GetLocalModerator();

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
            Moderator = nameof(LocalModerator),
            Mode = nameof(Mask),
            OriginAspNetRequestId = req.OriginAspNetRequestId,
            ProcessedContents = processedContents.ToArray()
        };

        return new OkObjectResult(response);
    }

    [FunctionName("LocalDetect")]
    public static async Task<IActionResult> Detect(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "local/detect")] Payload req, ILogger log)
    {
        log.LogInformation("C# HTTP trigger function local/detect processed a request.");

        var moderator = GetLocalModerator();
        var result = await moderator.HasBadWord(req.Contents.Select(p => p.RawText).ToArray());

        var response = new ModeratorResponse
        {
            Moderator = nameof(LocalModerator),
            Mode = nameof(Detect),
            OriginAspNetRequestId = req.OriginAspNetRequestId,
            ProcessedContents = null,
            Positive = result
        };

        return new OkObjectResult(response);
    }

    private static IModerator GetLocalModerator()
    {
        var words = Environment.GetEnvironmentVariable("Keywords");
        IModerator moderator = new LocalModerator(words);
        return moderator;
    }
}