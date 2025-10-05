namespace AssignmentPart1;
using System.Text.Json;

public class RequestValidator
{
    public Response ValidateRequest(Request request)
    {
        List<string> errors = new List<string>();
        string[] validMethods = new string[] { "create", "read", "update", "delete", "echo" };
        string[] methodsRequiringBody = new string[] { "create", "update", "echo" };
        string[] methodsRequiringJsonBody = new string[] { "create", "update" };

        if (string.IsNullOrWhiteSpace(request?.Method))
        {
            errors.Add("missing method");
        }
        else if (!validMethods.Contains(request.Method.ToLower()))
        {
            errors.Add("illegal method");
        }

        if (string.IsNullOrWhiteSpace(request?.Path))
        {
            errors.Add("missing path");
        }

        if (string.IsNullOrWhiteSpace(request?.Date))
        {
            errors.Add("missing date");
        }
        else if (!long.TryParse(request.Date, out long timestamp))
        {
            errors.Add("illegal date");
        }

        if (request?.Method != null && methodsRequiringBody.Contains(request.Method.ToLower()))
        {
            if (string.IsNullOrWhiteSpace(request?.Body))
            {
                errors.Add("missing body");
            }
            else if (methodsRequiringJsonBody.Contains(request.Method.ToLower()))
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
            return new Response { Status = "4 " + string.Join(", ", errors)};
        }

        return new Response { Status = "1 Ok"};
    }
}
