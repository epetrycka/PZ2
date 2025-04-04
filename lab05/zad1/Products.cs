using System;
using System.IO;
using System.Threading;

namespace lab5;

public struct Thread_params
{
    public int id { get; set; }
    public int sleep { get; set; }

    public Thread_params(int id, int sleep)
    {
        this.id = id;
        this.sleep = sleep;
    }
}

public struct Product_object
{
    public string data { get; set; }
    public int thread_id { get; set; }

    public Product_object(string data, int thread_id)
    {
        this.data = data;
        this.thread_id = thread_id;
    }
}

public class Products
{
    public static volatile bool running = true;
    public static List<Product_object> Product_object_list = new List<Product_object>();
    private static object lockObj = new object();

    public static void RunProducer(object threadParams)
    {
        Thread_params tp = (Thread_params)threadParams;

        while (running)
        {
            Product_object product = new Product_object("data", tp.id);

            lock (lockObj)
            {
                Product_object_list.Add(product);
            }

            Thread.Sleep(tp.sleep);
        }
    }

    public static void RunConsumer(object threadParams)
    {
        Thread_params tp = (Thread_params)threadParams;
        Dictionary<int, int> counter = new Dictionary<int, int>();

        while (running)
        {
            Product_object? product = null;

            lock (lockObj)
            {
                if (Product_object_list.Count > 0)
                {
                    product = Product_object_list[0];
                    Product_object_list.RemoveAt(0);
                }
            }

            if (product != null)
            {
                int prodId = product.Value.thread_id;
                if (!counter.ContainsKey(prodId)) counter[prodId] = 0;
                counter[prodId]++;
            }

            Thread.Sleep(tp.sleep);
        }

        Console.WriteLine($"Konsument {tp.id} statystyki:");
        foreach (var kv in counter)
        {
            Console.WriteLine($"Producent {kv.Key} - {kv.Value}");
        }
    }
}