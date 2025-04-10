using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace lab6;

public class ClientThread{
    private Socket clientSocket;
    private bool Running;
    private Task? receivingMessages = null;
    private Task? checkConnection = null;
    private Action<ClientThread>? EndCallback = null;
    private Action<string, ClientThread>? ReceivedMessageCallback = null;

    public ClientThread(Socket clientSocket, Action<ClientThread> EndCallback, Action<string, ClientThread> ReceivedMessageCallback){
        this.clientSocket = clientSocket;
        this.EndCallback = EndCallback;
        this.ReceivedMessageCallback = ReceivedMessageCallback;
        Running = true;
    }

    public String Name { get{ 
            IPEndPoint? remoteIpEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
            if (remoteIpEndPoint != null)
            {
                return "" + remoteIpEndPoint.Address + ":" + remoteIpEndPoint.Port;
            }
            else return "";
        }
    }

    public void Start(){
        receivingMessages = new Task(() => {
            string? data = null;
            byte[]? bufor = null;
            byte[] capacity = new byte[4];

            while (Running){
                int capacitySize = clientSocket.Receive(capacity, SocketFlags.None);
                String expectedSize = Encoding.UTF8.GetString(capacity, 0, capacitySize);

                bufor = new byte[int.Parse(expectedSize)];

                int size = clientSocket.Receive(bufor);
                if (size > 0){
                    data = Encoding.UTF8.GetString(bufor, 0, size);
                    if (ReceivedMessageCallback != null){
                        ReceivedMessageCallback(data, this);
                    } 
                }
            }
        });
        receivingMessages.Start();

        checkConnection = new Task(() => {
            while (Running) {
                if (!CheckConnection()){
                    this.KillThread();
                    Task.Delay(100).Wait();
                }
            }
        });
        checkConnection.Start();
    }

    public bool CheckConnection(){
        try {
            return !(clientSocket.Poll(1, SelectMode.SelectRead) 
            && clientSocket.Available == 0);
        }
        catch (SocketException) { return false; }
    }

    public void SendMessage(string data){
        Thread.Sleep(50);
        var encodedSize = Encoding.UTF8.GetBytes(data.Length.ToString());
        clientSocket.Send(encodedSize, 0);
        Thread.Sleep(100);

        Console.WriteLine(this.Name + ": sending message " + data);
        var encodedMessage = Encoding.UTF8.GetBytes(data);
        clientSocket.Send(encodedMessage, 0);
    }

    public void KillThread(){
        Console.WriteLine(this.Name + ": connection stopped");
        Running = false;
        try {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            if (EndCallback != null)
                EndCallback(this);
        }
        catch {}
    }
}