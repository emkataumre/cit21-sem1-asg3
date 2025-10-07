namespace Category;

public class UrlParser
{
    public bool HasId { get; private set; }
    public int Id { get; private set; }
    public string Path { get; private set; } = "";

    public bool ParseUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;

        string trimmed = url.Trim().TrimEnd('/'); // normalize trailing slash
        string[] parts = trimmed.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2 || !parts[0].Equals("api", StringComparison.OrdinalIgnoreCase)
        || !parts[1].Equals("categories", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (parts.Length == 2)
        {
            Path = "/api/categories";
            HasId = false;
            Id = 0;
            return true;
        }

        if (parts.Length == 3 && int.TryParse(parts[2], out int id) && id > 0)
        {
            Path = "/api/categories";
            HasId = true;
            Id = id;
            return true;
        }

        return false;
    }
}
