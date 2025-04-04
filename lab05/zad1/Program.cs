using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

using lab5;

class Program
{
    public static void Main(string[] args)
    {
        Random rand = new Random();
        var Producents_list = new List<Thread>();
        var Consumers_list = new List<Thread>();
        var n = 10;
        var m = 8;

        for (int i = 0; i < n; i++)
        {
            var threadParams = new Thread_params(i, rand.Next(10, 100));
            Thread newThread = new Thread(new ParameterizedThreadStart(Products.RunProducer));
            Producents_list.Add(newThread);
            newThread.Start(threadParams);
        }

        for (int i = 0; i < m; i++)
        {
            var threadParams = new Thread_params(i, rand.Next(10, 100));
            Thread newThread = new Thread(new ParameterizedThreadStart(Products.RunConsumer));
            Consumers_list.Add(newThread);
            newThread.Start(threadParams);
        }

        new Thread(() =>
        {
            while (Console.ReadKey(true).KeyChar != 'q') { }
            Products.running = false;
        }).Start();

        foreach (var t in Producents_list)
        {
            t.Join();
        }

        foreach (var t in Consumers_list)
        {
            t.Join();
        }

        Console.WriteLine("Dane nieskonsumowane:");
        Console.WriteLine(string.Join(", ", Products.Product_object_list.Select(p => $"[{p.thread_id}:{p.data}]")));
    }
}
