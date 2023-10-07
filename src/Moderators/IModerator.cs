namespace Moonglade.ContentSecurity.Moderators;

public interface IModerator
{
    public Task<string> ModerateContent(string input);

    public Task<bool> HasBadWord(params string[] input);
}