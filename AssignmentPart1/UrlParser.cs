using System;

namespace Category;

public class UrlParser
{
    public bool HasId { get; set; }
    public int Id { get; set; }
    public string Path { get; set; } = "";

    public bool ParseUrl(string url)
    {
        if (!RequestValidator.ValidatePath(url, out int? id, out string basePath)) return false;

        Path = basePath;

        if (id.HasValue)
        {
            HasId = true;
            Id = id.Value;
        }
        else
        {
            HasId = false;
        }

        return true;
    }

}