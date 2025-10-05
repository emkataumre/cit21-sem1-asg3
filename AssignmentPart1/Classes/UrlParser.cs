namespace AssignmentPart1;

public class UrlParser
{
    public bool HasId { get; set; }
    public int Id { get; set; }
    public string? Path { get; set; }

    public bool ParseUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        var trimmedUrl = url.Trim();
        var segments = trimmedUrl.Split('/');
        var lastSegment = segments.LastOrDefault();

        if (lastSegment is not null && int.TryParse(lastSegment, out int parsedId))
        {
            HasId = true;
            Id = parsedId;
            Path = string.Join("/", segments.SkipLast(1));
        }
        else
        {
            HasId = false;
            Id = 0;
            Path = trimmedUrl;
        }

        return true;
    }
}