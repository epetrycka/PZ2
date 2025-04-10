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

        Socket newSocket = new(
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp
        );

        newSocket.Bind(localEndPoint);
        newSocket.Listen(100);

        Socket klientSocket = newSocket.Accept();

        byte[] bufor = new byte[8];

        int received = klientSocket.Receive(bufor, SocketFlags.None);
        String wiadomosc = Encoding.UTF8.GetString(bufor, 0, received);
        // Console.WriteLine(wiadomosc);

        string odpowiedz = "Od serwera: Teraz wyslij wiadomosc o rozmiarze " + wiadomosc;
        var echoBytes = Encoding.UTF8.GetBytes(odpowiedz);
        klientSocket.Send(echoBytes, 0);

        byte[] bufor2 = new byte[int.Parse(wiadomosc)];

        int received2 = klientSocket.Receive(bufor2, SocketFlags.None);
        String wiadomosc2 = Encoding.UTF8.GetString(bufor2, 0, received2);

        Console.WriteLine(wiadomosc2);

        try{
            newSocket.Shutdown(SocketShutdown.Both);
            newSocket.Close();
        }
        catch{}
    }
}