using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using Moonglade.ContentSecurity.Moderators;

namespace Moonglade.ContentSecurity;

public static class LocalWordFilter
{
    [FunctionName("Mask")]
    public static async Task<IActionResult> Mask(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "local/mask")] Payload req, ILogger log)
    {
        log.LogInformation("C# HTTP trigger function Mask processed a request.");

        var words = Environment.GetEnvironmentVariable("Keywords");
        IModerator moderator = new LocalModerator(words);

        var result = await moderator.ModerateContent(req.Content);

        var response = new ModeratorResponse
        {
            Moderator = nameof(LocalModerator),
            Mode = nameof(Mask),
            OriginAspNetRequestId = req.OriginAspNetRequestId,
            ProcessedContent = result
        };

        return new OkObjectResult(response);
    }

    [FunctionName("Detect")]
    public static async Task<IActionResult> Detect(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "local/detect")] Payload req, ILogger log)
    {
        log.LogInformation("C# HTTP trigger function Detect processed a request.");

        var words = Environment.GetEnvironmentVariable("Keywords");
        IModerator moderator = new LocalModerator(words);

        return new OkResult();
    }
}