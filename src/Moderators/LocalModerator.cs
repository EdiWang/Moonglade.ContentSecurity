using Edi.WordFilter;

namespace Moonglade.ContentSecurity.Moderators;

public class LocalModerator : IModerator
{
    private readonly IMaskWordFilter _filter;

    public LocalModerator(string words)
    {
        var sw = new StringWordSource(words);

        IMaskWordFilter filter = new TrieTreeWordFilter(sw);
        _filter = filter;
    }

    public Task<string> ModerateContent(string input) => Task.FromResult(_filter.FilterContent(input));

    public Task<bool> HasBadWord(params string[] input) => Task.FromResult(input.Any(s => _filter.ContainsAnyWord(s)));
}