using System;
using System.IO;
using System.Threading;

class Program
{
    static volatile bool running = true;

    static void Main(string[] args)
    {
        string path = "Directory";

        if (!Directory.Exists(path))
        {
            Console.WriteLine("Podany katalog nie istnieje.");
            return;
        }

        Thread watcherThread = new Thread(() => MonitorDirectory(path));
        watcherThread.Start();

        new Thread(() =>
        {
            while (Console.ReadKey(true).KeyChar != 'q') { }
            running = false;
        }).Start();

        watcherThread.Join();
    }

    static void MonitorDirectory(string path)
    {
        using (FileSystemWatcher watcher = new FileSystemWatcher())
        {
            watcher.Path = path;
            watcher.IncludeSubdirectories = false;
            watcher.Filter = "*.*";
            watcher.NotifyFilter = NotifyFilters.FileName;

            watcher.Created += (s, e) =>
            {
                Console.WriteLine($"Dodano plik: {e.Name}");
            };

            watcher.Deleted += (s, e) =>
            {
                Console.WriteLine($"Usunięto plik: {e.Name}");
            };

            watcher.EnableRaisingEvents = true;

            while (running)
            {
                Thread.Sleep(100);
            }
        }
    }
}