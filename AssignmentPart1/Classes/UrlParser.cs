using System;

namespace Category;

public class UrlParser
{
    public bool HasId { get; set; }
    public int Id { get; set; }
    public string Path { get; set; } = "";

    public bool ParseUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        string trimmedUrl = url.Trim();
        string[] split = url.Split('/');
        string idSplit = split.LastOrDefault();

        if (!string.IsNullOrWhiteSpace(idSplit) && int.TryParse(idSplit, out int idInt))
        {
            HasId = true;
            Id = idInt;
            Path = string.Join("/", split.SkipLast(1));
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
//validate body
