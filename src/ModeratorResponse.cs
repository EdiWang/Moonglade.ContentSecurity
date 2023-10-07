namespace Moonglade.ContentSecurity;

public class ModeratorResponse
{
    public string OriginAspNetRequestId { get; set; }
    public string Moderator { get; set; }
    public string Mode { get; set; }
    public string ProcessedContent { get; set; }
}