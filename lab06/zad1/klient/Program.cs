using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace clientTCP;

class Program{
    public static void Main(string[] args){
        Console.WriteLine("Podaj treść do wysłania: ");
        string? doWyslania = Console.ReadLine();
        if (doWyslania != null) {
            if (doWyslania.Length > 1024){
                Console.WriteLine("Za duza wiadomosc");
                return;
            }
        }
        else {
            return;
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

        byte[] encodedWiadomosc = Encoding.UTF8.GetBytes(doWyslania);
        newSocket.Send(encodedWiadomosc, SocketFlags.None);

        var bufor = new byte[1_024];

        int bajty = newSocket.Receive(bufor, SocketFlags.None);
        string odpowiedz = Encoding.UTF8.GetString(bufor, 0, bajty);
        Console.WriteLine(odpowiedz);
        try{
            newSocket.Shutdown(SocketShutdown.Both);
            newSocket.Close();
        }
        catch{}
    }
}