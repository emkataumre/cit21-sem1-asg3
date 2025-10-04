using System;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Category;

public class Response
{
    public int Status { get; set; }
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

    public static Response ValidateRequest(Request request)
    {
        var validMethods = new List<string> { "create", "read", "update", "delete" };

        if (string.IsNullOrWhiteSpace(request.Method)) return new Response { Status = 4, Body = "missing method" };
        if (!validMethods.Contains(request.Method)) return new Response { Status = 4, Body = "illegal method" };
        if (string.IsNullOrWhiteSpace(request.Path)) return new Response { Status = 4, Body = "missing path" };
        if (string.IsNullOrWhiteSpace(request.Date)) return new Response { Status = 4, Body = "missing date" };
        if (!long.TryParse(request.Date, out long ts) || ts <= 0) return new Response { Status = 4, Body = "illegal date" };
        bool validPath = Regex.IsMatch(request.Path, @"^/api/categories(/\d+)?$");
        if (validPath == false) return new Response { Status = 4, Body = "illegal path" };


        return new Response { Status = 2, Body = "OK" };
    }
}