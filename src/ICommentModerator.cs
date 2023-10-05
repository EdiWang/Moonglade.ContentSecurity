using System.Threading.Tasks;

namespace Moonglade.ContentSecurity;

public interface ICommentModerator
{
    public Task<string> ModerateContent(string input);

    public Task<bool> HasBadWord(params string[] input);
}