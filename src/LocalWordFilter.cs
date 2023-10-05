using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Moonglade.ContentSecurity;

public static class LocalWordFilter
{
    [FunctionName("Mask")]
    public static async Task<IActionResult> Mask(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Payload req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function Mask processed a request.");

        return new OkResult();
    }

    [FunctionName("Detect")]
    public static async Task<IActionResult> Detect(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Payload req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function Detect processed a request.");

        return new OkResult();
    }
}