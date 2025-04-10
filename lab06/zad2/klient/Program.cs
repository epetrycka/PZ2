using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace clientTCP;

class Program{
    public static void Main(string[] args){
        Console.WriteLine("Podaj treść do wysłania: ");
        string? doWyslania = Console.ReadLine();
        int size_wiadomosci = 12;
        if (doWyslania != null) {
            size_wiadomosci += doWyslania.Length;
        }

        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        Socket newSocket = new (
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp
        );

        newSocket.Connect(localEndPoint);

        string wiadomosc = size_wiadomosci.ToString();
        byte[] encodedWiadomosc = Encoding.UTF8.GetBytes(wiadomosc);
        newSocket.Send(encodedWiadomosc, SocketFlags.None);

        var bufor = new byte[1_024];

        int bajty = newSocket.Receive(bufor, SocketFlags.None);
        string odpowiedz = Encoding.UTF8.GetString(bufor, 0, bajty);
        Console.WriteLine(odpowiedz);

        string wiadomosc2 = "Od klienta: " + doWyslania;
        byte[] encodedWiadomosc2 = Encoding.UTF8.GetBytes(wiadomosc2);
        newSocket.Send(encodedWiadomosc2, SocketFlags.None);

        try{
            newSocket.Shutdown(SocketShutdown.Both);
            newSocket.Close();
        }
        catch{}
    }
}