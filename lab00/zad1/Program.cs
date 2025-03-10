using System;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        int repeat = 0;

        try
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("No arguments provided");
            }

            repeat = int.Parse(args.Last());
        }
        catch (FormatException e)
        {
            Console.WriteLine("Wrong format error: " + e.Message);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("No arguments provided error: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Unexpected error: " + e.Message);
        }

        for (int i=0; i<repeat; i++){
            Console.WriteLine(string.Join(" ", args[..^1]));
        }
    }
}