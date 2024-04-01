using Azure.AI.ContentSafety;

namespace Moonglade.ContentSecurity.Moderators;

public class AzureAIContentSafetyModerator : IModerator
{
    private readonly ContentSafetyClient _client;

    public AzureAIContentSafetyModerator(ContentSafetyClient client) => _client = client;

    public async Task<string> ModerateContent(string input)
    {
        var request = new AnalyzeTextOptions(input);
        request.BlocklistNames.Add("Moonglade.ContentSecurity.BlockList");
        //request.HaltOnBlocklistHit = true;

        var screenResult = await _client.AnalyzeTextAsync(request);

        if (screenResult.Value is not null)
        {
            // BlocklistMatch, replace with *
            input = screenResult.Value.BlocklistsMatch.Aggregate(input,
                (current, item) => current.Replace(item.BlocklistItemText, "*"));

            var positiveCats = screenResult.Value.CategoriesAnalysis.Where(item => item.Severity > 0)
                .Select(item => item.Category.ToString())
                .ToList();

            if (positiveCats.Count > 0)
            {
                input = $"[Content blocked due to: {string.Join(", ", positiveCats)}]";
            }
        }

        return input;
    }

    public async Task<bool> HasBadWord(params string[] input)
    {
        foreach (var s in input)
        {
            var request = new AnalyzeTextOptions(s);
            request.BlocklistNames.Add("Moonglade.ContentSecurity.BlockList");
            //request.HaltOnBlocklistHit = true;

            var screenResult = await _client.AnalyzeTextAsync(request);
            return screenResult.Value is not null &&
                   (screenResult.Value.BlocklistsMatch.Any() ||
                    screenResult.Value.CategoriesAnalysis.Any(item => item.Severity > 0));
        }

        return false;
    }
}