using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

using lab6;

class Program{
    public static void Main(string[] args){
        Server server = new Server();

        Task serverBackground = new Task(() => server.Run());
        serverBackground.Start();

        serverBackground.Wait();
        Console.WriteLine("Server shutdown");
    }
}