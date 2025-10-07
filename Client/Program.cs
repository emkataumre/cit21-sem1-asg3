using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Category;

public class Client
{
    static void Main()
    {
        TcpClient client = new TcpClient();
        client.Connect(IPAddress.Loopback, 5000);
        Console.WriteLine("Client: connected to server.");

        using NetworkStream stream = client.GetStream();

        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

        // 1) READ — list (no JSON body)
        var readList = new Request
        {
            Method = "read",
            Path = "/api/categories",
            Date = now,
            Body = ""
        };

        // 2) READ — single (no JSON body)
        var readOne = new Request
        {
            Method = "read",
            Path = "/api/categories/1",
            Date = now,
            Body = ""
        };

        // 3) CREATE — valid (JSON via serializer; your server expects Id + Name)
        var create = new Request
        {
            Method = "create",
            Path = "/api/categories",
            Date = now,
            Body = JsonSerializer.Serialize(new { Id = 4, Name = "Snacks" })
        };

        // 4) UPDATE — valid (JSON via serializer; only Name is needed in your code)
        var update = new Request
        {
            Method = "update",
            Path = "/api/categories/4",
            Date = now,
            Body = JsonSerializer.Serialize(new { Name = "Beverages & Tea" })
        };

        // 5) DELETE — valid (no JSON body)
        var deleteReq = new Request
        {
            Method = "delete",
            Path = "/api/categories/4",
            Date = now,
            Body = ""
        };

        // 6) ECHO — valid (PLAIN TEXT to match spec; avoid serializing here)
        var echo = new Request
        {
            Method = "echo",
            Path = "/api/categories",
            Date = now,
            Body = "hello from client"
        };


        var messageJson = JsonSerializer.Serialize(readOne);

        byte[] messageBytes = Encoding.UTF8.GetBytes(messageJson);
        stream.Write(messageBytes, 0, messageBytes.Length);

        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string responseString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Response response = JsonSerializer.Deserialize<Response>(responseString)!;
        Console.WriteLine($"Client: received message: {response.Status} {response.Body}");
    }
}