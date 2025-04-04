using System;
using System.Threading;

class Program
{
    static int n = 8;
    static bool[] started = new bool[n];
    static bool exit = false;
    static object lockObj = new object();

    static void Main()
    {
        Thread[] threads = new Thread[n];

        for (int i = 0; i < n; i++)
        {
            int index = i;
            threads[i] = new Thread(() => Worker(index));
            threads[i].Start();
        }

        while (true)
        {
            bool allStarted = true;
            lock (lockObj)
            {
                for (int i = 0; i < n; i++)
                {
                    if (!started[i])
                    {
                        allStarted = false;
                        break;
                    }
                }
            }

            if (allStarted)
            {
                Console.WriteLine("Wszystkie wątki wystartowały.");
                break;
            }

            Thread.Sleep(2000);
        }

        lock (lockObj)
        {
            exit = true;
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("Wszystkie wątki zakończone.");
    }

    static void Worker(int index)
    {
        lock (lockObj)
        {
            started[index] = true;
        }

        Console.WriteLine($"[Wątek {index}] rozpoczął");

        while (true)
        {
            lock (lockObj)
            {
                if (exit)
                {
                    break;
                }
            }

            Thread.Sleep(200);
        }

        Console.WriteLine($"[Wątek {index}] skończył");
    }
}
