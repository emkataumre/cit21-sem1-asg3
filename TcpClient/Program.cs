using System.Net.Sockets;

static void Connect()
{
    try
    {
        Int32 port = 5000;
        Console.Write("Enter message: ");
        string message = Console.ReadLine();
        
        using TcpClient client = new TcpClient("127.0.0.1", port);
        
        Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
        
        NetworkStream stream = client.GetStream();
        
        stream.Write(data, 0, data.Length);

        Console.WriteLine("Sent: {0}", message);
        
        data = new Byte[256];
        
        String responseData = String.Empty;
        
        Int32 bytes = stream.Read(data, 0, data.Length);
        responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
        Console.WriteLine("Received: {0}", responseData);
    }
    catch (ArgumentNullException e)
    {
        Console.WriteLine("ArgumentNullException: {0}", e);
    }
    catch (SocketException e)
    {
        Console.WriteLine("SocketException: {0}", e);
    }

    Console.WriteLine("\n Press Enter to continue...");
    Console.Read();
    
}

Connect();