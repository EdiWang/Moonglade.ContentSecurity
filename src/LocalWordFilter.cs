using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moonglade.ContentSecurity.Moderators;

namespace Moonglade.ContentSecurity;

public class LocalWordFilter(ILogger<LocalWordFilter> logger)
{
    [Function("LocalMask")]
    public async Task<IActionResult> Mask(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "local/mask")] Payload req)
    {
        logger.LogInformation("C# HTTP trigger function local/mask processed a request.");

        var moderator = GetLocalModerator();

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
            Moderator = nameof(LocalModerator),
            Mode = nameof(Mask),
            OriginAspNetRequestId = req.OriginAspNetRequestId,
            ProcessedContents = processedContents.ToArray()
        };

        return new OkObjectResult(response);
    }

    [Function("LocalDetect")]
    public async Task<IActionResult> Detect(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "local/detect")] Payload req)
    {
        logger.LogInformation("C# HTTP trigger function local/detect processed a request.");

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