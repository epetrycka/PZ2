using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace lab6;

public class Server{
    private IPHostEntry host;
    private IPAddress ipAddress;
    private IPEndPoint localEndPoint;
    private Socket socket;
    public string my_dir;
    private bool Running;
    public List<ClientThread> clientThreads; 

    public Server()
    {
        host = Dns.GetHostEntry("localhost");
        ipAddress = host.AddressList[0];
        localEndPoint = new IPEndPoint(ipAddress, 11000);
        socket = new Socket(
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp
        );
        my_dir = Directory.GetCurrentDirectory();
        Running = true;
        clientThreads = new List<ClientThread>();
    }

    public void Run(){
        new Thread(() => {
        Console.WriteLine("Press 'q' to stop the server");
            while (Running){
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q){
                    Console.WriteLine("Shutting down server...");
                    Running = false;
                    socket.Close();
                }
                Thread.Sleep(100); 
            }
        }).Start();

        try {
        socket.Bind(localEndPoint);
        socket.Listen(100);
        Console.WriteLine("Server started and listening");

        Task runningServer = new Task(() => {
            while (Running){
                Socket clientSocket = socket.Accept();
                Console.WriteLine("Connected with client");
                ClientThread client = new ClientThread(clientSocket, EndClient, ReceivedMessage);
                Task handleClient = new Task(client.Start);
                handleClient.Start();
                clientThreads.Add(client);
            }
        });
        runningServer.Start();

        runningServer.Wait();
        }
        catch {
            Console.WriteLine("Exception caught during running server");
        }
    }

    public void EndClient(ClientThread client){
        Monitor.Enter(clientThreads);
        try {
                if (clientThreads.Contains(client))
                    clientThreads.Remove(client);
        }
        finally{
            Monitor.Exit(clientThreads);
        }
    }

    public void ReceivedMessage(string data, ClientThread client){
        Console.WriteLine($"Received message: {data}");
        if (data == "!end"){
            client.KillThread();
            this.Kill();
        }
        else if (data == "list"){
            BlockingCollection<string> foundFiles = new BlockingCollection<string>();

            Thread searchThread = new Thread(() => {
                try{
                    foreach (string file in Directory.EnumerateFileSystemEntries(my_dir))
                    {
                        foundFiles.Add(Path.GetFileName(file));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error while searching: {e.Message}");
                }
                finally
                {
                    foundFiles.CompleteAdding();
                }
            });
            searchThread.Start();
            searchThread.Join();

            client.SendMessage(string.Join(", \n", foundFiles));
        }
        else if (data.StartsWith("in ")) {
            BlockingCollection<string> foundFiles = new BlockingCollection<string>();

            Thread searchThread = new Thread(() => {
                try {
                    string folderName = data.Substring(3).Trim();

                    if (folderName == "..") {
                        string? parentDir = Directory.GetParent(my_dir)?.FullName;

                        if (parentDir != null) {
                            string currentDir = parentDir;
                            Console.WriteLine($"Wchodzę do katalogu nadrzędnego: {currentDir}");

                            var foundEntries = Directory.EnumerateFileSystemEntries(currentDir);
                            foreach (var entry in foundEntries) {
                                foundFiles.Add(Path.GetFileName(entry));
                            }
                        } else {
                            client.SendMessage("Directory does not exist");
                            return;
                        }
                    }
                    else {
                        string potentialDir = Path.Combine(my_dir, folderName);

                        if (Directory.Exists(potentialDir)) {
                            string currentDir = potentialDir;
                            Console.WriteLine($"Wchodzę do katalogu: {currentDir}");
                            
                            var foundEntries = Directory.EnumerateFileSystemEntries(currentDir);
                            foreach (var entry in foundEntries) {
                                foundFiles.Add(Path.GetFileName(entry));
                            }
                        } else {
                            client.SendMessage("Directory does not exist");
                            return;
                        }
                    }
                }
                catch (Exception e) {
                    Console.WriteLine($"Błąd: {e.Message}");
                }
                finally {
                    foundFiles.CompleteAdding();
                }
            });
            searchThread.Start();
            searchThread.Join();

            client.SendMessage(string.Join(", \n", foundFiles));
        }
        else {
            client.SendMessage("nieznane polecenie");
        }
    }

    public void Kill(){
        try
            {
                Console.WriteLine("Server is shutting down");
                Monitor.Enter(clientThreads);
                try {
                    foreach(ClientThread client in clientThreads)
                    {
                        client.KillThread();
                    }
                }
                finally{
                    Monitor.Exit(clientThreads);
                }
                socket.Close();
                socket.Shutdown(SocketShutdown.Both);
            }
        catch {}
        Running = false;
    }
}
