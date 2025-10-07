using System.Net;
using System.Net.Sockets;
using System.Text.Json.Nodes;
using System.Text;
using System.Text.Json;
using AssignmentPart1;

static string ToJson(object data)
{
    return JsonSerializer.Serialize(data,
        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
}

TcpListener server = null;
try
{
    var port = 5000;
    var localAddr = IPAddress.Parse("127.0.0.1");

    server = new TcpListener(localAddr, port);
    server.Start();

    var catService = new CategoryService();

    while (true)
    {
        Console.Write("Waiting for a connection... ");
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Connected!");
        
        var clientThread = new Thread(() => HandleClient(client, catService));
        clientThread.Start();
    }
}
catch (SocketException e)
{
    Console.WriteLine($"SocketException: {e}");
}
finally
{
    server?.Stop();
    Console.WriteLine("Server stopped.");
}

void HandleClient(TcpClient client, CategoryService catService)
{
    try
    {
        using (client)
        using (NetworkStream stream = client.GetStream())
        {
            byte[] buffer = new byte[2048];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead == 0) return;

            string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {data}");

            var requestValidator = new RequestValidator();
            Response response = new Response();

            var json = JsonObject.Parse(data);
            var request = new Request
            {
                Method = json["method"]?.ToString(),
                Path = json["path"]?.ToString(),
                Date = json["date"]?.ToString(),
                Body = json["body"]?.ToString()
            };

            var validation = requestValidator.ValidateRequest(request);
            response.Status = validation.Status;

            if (validation.Status != "1 Ok")
            {
                byte[] msg = Encoding.UTF8.GetBytes(ToJson(response));
                stream.Write(msg, 0, msg.Length);
                return;
            }

            var urlParser = new UrlParser();
            urlParser.ParseUrl(request.Path);

            switch (request.Method)
            {
                case "echo":
                    response.Status = "1 Ok";
                    response.Body = request.Body;
                    break;

                case "read":
                    if (urlParser.HasId)
                    {
                        var category = catService.GetCategory(urlParser.Id);
                        if (category is not null)
                        {
                            response.Status = "1 Ok";
                            response.Body = ToJson(category);
                        }
                        else
                        {
                            response.Status = "5 Not found";
                        }
                    }
                    else
                    {
                        var categories = catService.GetCategories();
                        response.Status = "1 Ok";
                        response.Body = ToJson(categories);
                    }

                    break;

                case "create":
                    if (urlParser.HasId)
                    {
                        response.Status = "4 Bad Request";
                        break;
                    }

                    var jsonBody = JsonObject.Parse(request.Body);
                    var name = jsonBody["name"]?.ToString();

                    catService.CreateCategory(null, name);
                    var all = catService.GetCategories();
                    var newCategory = all[^1];

                    response.Status = "2 Created";
                    response.Body = ToJson(newCategory);
                    break;

                case "update":
                    if (!urlParser.HasId)
                    {
                        response.Status = "4 Bad Request";
                        break;
                    }

                    var existing = catService.GetCategory(urlParser.Id);
                    if (existing is null)
                    {
                        response.Status = "5 Not found";
                        break;
                    }

                    var updateJson = JsonObject.Parse(request.Body);
                    var newName = updateJson["name"]?.ToString();
                    catService.UpdateCategory(urlParser.Id, newName);
                    response.Status = "3 Updated";
                    break;

                case "delete":
                    if (!urlParser.HasId)
                    {
                        response.Status = "4 Bad Request";
                        break;
                    }

                    var cat = catService.GetCategory(urlParser.Id);
                    if (cat is null)
                    {
                        response.Status = "5 Not found";
                        break;
                    }

                    catService.DeleteCategory(urlParser.Id);
                    response.Status = "1 Ok";
                    break;
            }

            byte[] responseMsg = Encoding.UTF8.GetBytes(ToJson(response));
            stream.Write(responseMsg, 0, responseMsg.Length);
            stream.Flush();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error handling client: {ex.Message}");
    }
}

Console.WriteLine("\nHit enter to continue...");
Console.ReadLine();