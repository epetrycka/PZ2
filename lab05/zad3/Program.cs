using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

class Program
{
    static volatile bool searching = true;
    static BlockingCollection<string> foundFiles = new BlockingCollection<string>();

    static void Main(string[] args)
    {
        string startPath = "Directory";

        if (!Directory.Exists(startPath))
        {
            Console.WriteLine("Podany katalog nie istnieje.");
            return;
        }

        string pattern = "makaron";

        Thread searchThread = new Thread(() => SearchFiles(startPath, pattern));
        searchThread.Start();

        while (searching || foundFiles.Count > 0)
        {
            if (foundFiles.TryTake(out string filePath, Timeout.Infinite))
            {
                Console.WriteLine($"Znaleziono: {filePath}");
            }
        }

        searchThread.Join();
    }

    static void SearchFiles(string path, string pattern)
    {
        try
        {
            foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
            {
                if (file.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    foundFiles.Add(file);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Błąd podczas przeszukiwania: {e.Message}");
        }
        finally
        {
            searching = false;
            foundFiles.CompleteAdding();
        }
    }
}
