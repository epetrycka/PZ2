using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using lab6;

class Program{
    public static void Main(string[] args){
        Client client = new Client();
        client.Run().Wait();
        Console.WriteLine("Server not reachable");
    }
}