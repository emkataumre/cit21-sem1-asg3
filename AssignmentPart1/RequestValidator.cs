using System.Text.Json;
using System.Text.Json.Nodes;

namespace AssignmentPart1;

public class Response
{
    public string Status { get; set; }
    public string Body { get; set; }
}

public class Request
{
    public string Method { get; set; }
    public string Path { get; set; }
    public string Date { get; set; }
    public string? Body { get; set; }
    
}

public class RequestValidator
{
    public Response ValidateRequest(Request request)
    { var validMethods = new List<string> { "create", "read", "update", "delete", "echo" };
        var response = new Response() { Status = "1 Ok" };

        if (string.IsNullOrWhiteSpace(request.Date))
        {
            response.Status = "4 missing date";
            return response;
        }
        
        if (string.IsNullOrWhiteSpace(request.Method))
        {
            response.Status = "4 missing method";
            return response;
        }

        if (string.IsNullOrWhiteSpace(request.Path) && request.Method != "echo")
        {
            response.Status = "4 missing path";
            return response;
        }
        
        if (!validMethods.Contains(request.Method))
        {
            response.Status = "4 illegal method";
            return response;
        }

        if (!long.TryParse(request.Date, out _))
        {
            response.Status = "4 illegal date";
            return response;
        }
        
        if ((request.Method is "create" or "update" or "echo") && string.IsNullOrWhiteSpace(request.Body))
        {
            response.Status = "4 missing body";
            return response;
        }
        
        if ((request.Method is "create" or "update") && request.Body != null && !IsValidJson(request.Body))
        {
            response.Status = "4 illegal body";
            return response;
        }
        
        if (request.Method != "echo")
        {
            if (!request.Path.StartsWith("/api/categories"))
            {
                response.Status = "5 Not found";
                return response;
            }
            
            var parts = request.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length > 2)
            {
                if (!int.TryParse(parts[2], out _))
                {
                    response.Status = "4 Bad Request";
                    return response;
                }
            }
        }

        return response;
    }
    
    public bool IsValidJson(string? jsonString)
    {
        try
        {
            JsonObject.Parse(jsonString);
            return true;
        }
        catch
        {
            return false;
        }
    }
}