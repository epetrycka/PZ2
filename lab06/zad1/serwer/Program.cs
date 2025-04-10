using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace serverTCP;

class Program{
    public static void Main(string[] args){
        IPHostEntry host = Dns.GetHostEntry("localhost");

        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        // Console.WriteLine(localEndPoint.AddressFamily);

        Socket newSocket = new(
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp
        );

        newSocket.Bind(localEndPoint);
        newSocket.Listen(100);

        Socket klientSocket = newSocket.Accept();

        byte[] bufor = new byte[1_024];

        int received = klientSocket.Receive(bufor, SocketFlags.None);
        String wiadomosc = Encoding.UTF8.GetString(bufor, 0, received);
        Console.WriteLine(wiadomosc);

        string odpowiedz = "odczytalem: " + wiadomosc;
        var echoBytes = Encoding.UTF8.GetBytes(odpowiedz);
        klientSocket.Send(echoBytes, 0);

        try{
            newSocket.Shutdown(SocketShutdown.Both);
            newSocket.Close();
        }
        catch{}
    }
}