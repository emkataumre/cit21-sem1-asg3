using System.Text.Json;
using System.Text.Json.Nodes;

namespace AssignmentPart1;

public class Response
{
    public required string Status { get; set; }
    
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
    {
        List<string> validMethods = new List<string>() { "create", "read", "update", "delete", "echo" };
        
        var response = new Response() { Status = "1 Ok",};
        
        if (request.Method == null)
        {
            response.Status = "4 missing method";
        }

        if (!validMethods.Contains(request.Method) && request.Method != null)
        {
            response.Status = "4 illegal method";
        }

        if (request.Path == null || request.Path == "")
        {
            response.Status = "4 missing path";
        }
        
        if (request.Date == null || request.Date == "")
        {
            response.Status = "4 missing date";
        }

        if (request.Date != null && !long.TryParse(request.Date, out var result))
        {
            response.Status = "4 illegal date";
        }

        if ((request.Method is "update" or "create" or "echo") && request.Body == null)
        {
            response.Status = "4 missing body";
        }

        if ((request.Method is "update" or "create") && request.Body != null && !IsValidJson(request.Body))
        {
            response.Status = "4 illegal body";
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
        catch (JsonException)
        {
            return false;
        }
    }
}