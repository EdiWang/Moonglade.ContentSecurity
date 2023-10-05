using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Moonglade.ContentSecurity;

public static class LocalWordFilter
{
    [FunctionName("LocalWordFilter")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Payload req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function LocalWordFilter processed a request.");

        return new OkResult();
    }
}