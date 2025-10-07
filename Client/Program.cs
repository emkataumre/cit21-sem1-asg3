using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Category;

public class Client
{
    static void Main()
    {
        TcpClient client = new TcpClient();
        client.Connect(IPAddress.Loopback, 5000);
        Console.WriteLine("Client: connected to server.");

        using NetworkStream stream = client.GetStream();

        string message = "Hi I am client!";
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        stream.Write(messageBytes, 0, messageBytes.Length);

        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine($"Client: received message: {response}");
    }
}