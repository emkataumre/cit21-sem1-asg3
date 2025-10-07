using System.Text.Json;

namespace Category;

public class RequestValidator
{
    public Response ValidateRequest(Request request)
    {
        string[] validMethods = ["create", "read", "update", "delete", "echo"];
        string[] methodsRequiringBody = ["create", "update", "echo"];
        string[] methodsRequiringJsonBody = ["create", "update"];
        List<string> errors = new List<string>();

        if (request is null)
        {
            return new Response { Status = "4", Body = "illegal request" };
        }

        if (string.IsNullOrWhiteSpace(request.Method))
        {
            errors.Add("4 missing method");
        }
        else if (!validMethods.Contains(request.Method))
        {
            errors.Add("4 illegal method");
        }

        if (string.IsNullOrWhiteSpace(request.Path))
        {
            errors.Add("4 missing path");

        }

        if (string.IsNullOrWhiteSpace(request.Date))
        {
            errors.Add("4 missing date");
        }
        else if (!long.TryParse(request.Date, out long ts) || ts <= 0)
        {
            errors.Add("4 illegal date");
        }


        if (!string.IsNullOrWhiteSpace(request.Method) && methodsRequiringBody.Contains(request.Method))
        {
            if (string.IsNullOrWhiteSpace(request.Body))
            {
                errors.Add("4 missing body");
            }
            else if (methodsRequiringJsonBody.Contains(request.Method))
            {
                try
                {
                    JsonDocument.Parse(request.Body);
                }
                catch
                {
                    errors.Add("illegal body");
                }
            }
        }


        if (errors.Count > 0)
        {
            return new Response { Status = "4", Body = string.Join(",", errors) };
        }

        return new Response { Status = "1", Body = "Ok" };
    }
}
