namespace AssignmentPart1;

public class UrlParser {
    public bool HasId { get; set; }
    public int Id { get; set; }
    public string Path { get; set; }
    
    public bool ParseUrl(string url)
    {
        bool isValid = Uri.IsWellFormedUriString(url, UriKind.Relative);

        if (!isValid)
        {
            return false;
        }
        
        string[] urlParts = url.Split('/', StringSplitOptions.RemoveEmptyEntries);
        
        if (urlParts.Length == 3)
        {
            if (!Int32.TryParse(urlParts[2], out int result))
            {
                return false;
            }
            
            HasId = true;
            Id = Convert.ToInt32(urlParts[2]);
        }

        for (int i = 0; i < urlParts.Length; i++)
        {
            // exit loop early if id is part of the path
            if (i == 2)
            {
                break;
            }
            Path = Path + "/" + urlParts[i];
        }
        
        return true;
    }
}