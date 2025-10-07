using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Category;

public class Server
{
    // unified JSON options: camelCase out, case-insensitive in
    static readonly JsonSerializerOptions Json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    static async Task Main()
    {
        const int port = 5000;

        var server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine($"Server: listening on port {port}");

        while (true)
        {
            TcpClient client = await server.AcceptTcpClientAsync();
            Console.WriteLine("Client connected");
            _ = HandleClientAsync(client);
        }
    }

    // helpers
    static Response MakeResponse(string status, string? body = null) => new() { Status = status, Body = body };

    static async Task SendResponseAsync(NetworkStream stream, Response res)
    {
        if (stream is null || !stream.CanWrite) return;
        string json = JsonSerializer.Serialize(res, Json);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        await stream.WriteAsync(bytes, 0, bytes.Length);
        await stream.FlushAsync();
    }

    static bool TryDeserialize<T>(string? json, out T? value)
    {
        try
        {
            value = JsonSerializer.Deserialize<T>(json!, Json);
            return value is not null;
        }
        catch
        {
            value = default;
            return false;
        }
    }

    static async Task SendAndReturnAsync(NetworkStream stream, string status, string? body = null)
        => await SendResponseAsync(stream, MakeResponse(status, body));

    static async Task HandleClientAsync(TcpClient client)
    {
        var requestValidator = new RequestValidator();
        var urlParser = new UrlParser();
        var categoryService = new CategoryService();

        NetworkStream? stream = null;

        try
        {
            stream = client.GetStream();

            byte[] buffer = new byte[4096];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string requestJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Server: received message {requestJson}");

            if (!TryDeserialize<Request>(requestJson, out var request))
            {
                await SendAndReturnAsync(stream, "4 Bad Request");
                return;
            }

            Response vr = requestValidator.ValidateRequest(request);
            if (vr.Status == "4")
            {
                await SendAndReturnAsync(stream, $"4 {vr.Body}");
                return;
            }

            var response = new Response();

            if (!urlParser.ParseUrl(request.Path))
            {
                response = MakeResponse("5 Not found");
                await SendAndReturnAsync(stream, "5 Not found");
                return;
            }

            bool hasId = urlParser.HasId;
            int id = urlParser.Id;


            switch (request.Method)
            {
                case "read":
                    {
                        if (hasId)
                        {
                            var category = categoryService.GetCategory(id);
                            response = category is null
                                ? MakeResponse("5 Not found")
                                : MakeResponse("1 Ok", JsonSerializer.Serialize(category, Json));
                        }
                        else
                        {
                            var categories = categoryService.GetCategories();
                            response = categories.Count == 0
                                ? MakeResponse("5 Not found")
                                : MakeResponse("1 Ok", JsonSerializer.Serialize(categories, Json));
                        }
                        break;
                    }

                case "create":
                    {
                        if (hasId)
                        {
                            await SendAndReturnAsync(stream, "4 Bad Request");
                            return;
                        }

                        if (!TryDeserialize<Category>(request.Body, out var newCat))
                        {
                            response = MakeResponse("4 Bad Request");
                            break;
                        }

                        bool created = categoryService.CreateCategory(newCat.Name, out Category createdCategory);
                        response = created
                            ? MakeResponse("2 Created", JsonSerializer.Serialize(createdCategory, Json))
                            : MakeResponse("4 Bad Request");
                        Console.WriteLine($"hereeeeeee {createdCategory.Id}");
                        break;
                    }

                case "update":
                    {
                        if (!hasId)
                        {
                            await SendAndReturnAsync(stream, "4 Bad Request");
                            return;
                        }

                        var existing = categoryService.GetCategory(id);
                        if (existing is null)
                        {
                            response = MakeResponse("5 Not found");
                            break;
                        }

                        if (!TryDeserialize<Category>(request.Body, out var updated))
                        {
                            response = MakeResponse("4 Bad Request");
                            break;
                        }

                        categoryService.UpdateCategory(id, updated.Name);
                        response = MakeResponse("3 Updated");
                        break;
                    }

                case "delete":
                    {
                        if (!hasId)
                        {
                            await SendAndReturnAsync(stream, "4 Bad Request");
                            return;
                        }

                        var existing = categoryService.GetCategory(id);
                        if (existing is null)
                        {
                            response = MakeResponse("5 Not found");
                            break;
                        }

                        categoryService.DeleteCategory(id);
                        response = MakeResponse("1 Ok");
                        break;
                    }

                case "echo":
                    {
                        response = MakeResponse("1 Ok", request.Body ?? null);
                        break;
                    }
            }

            await SendResponseAsync(stream, response);
            Console.WriteLine($"response {response.Status}, {response.Body}");
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"Server json exception: {jsonEx}");
            if (stream is not null) await SendAndReturnAsync(stream, "4 Bad Request");
        }
        catch (IOException ioEx)
        {
            Console.WriteLine($"Server error: {ioEx}");
            if (stream is not null) await SendAndReturnAsync(stream, "6 Error");
        }
        finally
        {
            stream?.Dispose();
            client.Close();
        }
    }
}
