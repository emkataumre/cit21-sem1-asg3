using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;
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

        if (!Regex.IsMatch(request.Path, @"^/api/categories(/\d+)?$"))
            return MakeResponse(4, "illegal path");

        return MakeResponse(1, "Ok");
    }
}
