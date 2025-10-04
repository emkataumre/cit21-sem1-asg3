using System;
using System.Text.RegularExpressions;

namespace Category;

public class Response
{
    public int Status { get; set; }
    public string? Body { get; set; }

}

public class Request
{
    public required string Method { get; set; }
    public required string Path { get; set; }
    public required string Date { get; set; }
    public string? Body { get; set; }
}

public class RequestValidator
{

    public static bool ValidatePath(string path, out int? id, out string basePath)
    {
        basePath = "";
        id = null;
        var match = Regex.Match(path, @"^(/api/categories)(?:/(\d+))?$");

        if (!match.Success) return false;

        basePath = match.Groups[1].Value;

        if (match.Groups[2].Success)
        {
            id = int.Parse(match.Groups[2].Value);
        }

        return true;
    }

    private static Response MakeResponse(int status, string body)
    {
        Response resp = new Response { Status = status, Body = body };
        Console.WriteLine(status + " " + body);
        return resp;
    }
    public static Response ValidateRequest(Request request)
    {
        var validMethods = new List<string> { "create", "read", "update", "delete" };

        if (string.IsNullOrWhiteSpace(request.Method)) return MakeResponse(4, "missing method");
        if (!validMethods.Contains(request.Method)) return MakeResponse(4, "illegal method");
        if (string.IsNullOrWhiteSpace(request.Path)) return MakeResponse(4, "missing path");
        if (string.IsNullOrWhiteSpace(request.Date)) return MakeResponse(4, "missing date");
        if (!long.TryParse(request.Date, out long ts) || ts <= 0)
            return MakeResponse(4, "illegal date");

        if (!ValidatePath(request.Path, out int? id, out string basePath)) return MakeResponse(4, "illegal path");

        return MakeResponse(1, "Ok");
    }
}
