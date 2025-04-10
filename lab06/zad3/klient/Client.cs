using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace lab6;

public class Client{
    private IPHostEntry host;
    private IPAddress ipAddress;
    private IPEndPoint localEndPoint;
    private Socket socket;
    public bool Running { get; set; }
    private Task? communication = null;
    private CancellationTokenSource _cts = new();
    private CancellationToken _token;


    public Client(){
        host = Dns.GetHostEntry("localhost");
        ipAddress = host.AddressList[0];
        localEndPoint = new IPEndPoint(ipAddress, 11000);

        socket = new (
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp
        );

        Running = true;
        _token = _cts.Token;
    }

    public async Task Run(){
        try {
            socket.Connect(localEndPoint);
            Console.WriteLine("Connected with server");
            ClientThread client = new ClientThread(socket, EndThread, ReceivedMessage);

            Task handleClient = Task.Run(() => client.Start());

            communication = Task.Run(async () => {
                while (Running)
                {
                    Thread.Sleep(1000);
                    Console.Write("Type message to server: ");
                    string? data = await Task.Run(() => Console.ReadLine(), _token);

                    if (_token.IsCancellationRequested)
                        break;

                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        client.SendMessage(data);
                    }

                    await Task.Delay(100, _token);
                }
            }, _token);

            await Task.WhenAll(handleClient, communication);
        }
        catch {
            Console.WriteLine("Exception caught during running host");
        }
        finally {
            Console.WriteLine("Client finished");
        }
    }

    public void EndThread(ClientThread client){
        Console.WriteLine("Server stopped connection with the host");
        this.Kill();
    }

    public void ReceivedMessage(string data, ClientThread client){
        Console.WriteLine($"Received message: {data}");
    }

    public void Kill(){
        try{
            Console.WriteLine("Client is shutting down");
            _cts.Cancel();
            socket.Close();
            socket.Shutdown(SocketShutdown.Both);
        }
        catch {}
        Running = false;
    }
}