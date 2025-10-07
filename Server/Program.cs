using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Category;

public class Server
{
    static void Main()
    {
        int port = 5000;

        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine($"Server: listening on port {port}");


        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected");

            using NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Server: received message {received}");

            string msg = "Hi I am server!";
            byte[] responseBytes = Encoding.UTF8.GetBytes(msg);
            stream.Write(Encoding.UTF8.GetBytes(msg));
        }
    }
}