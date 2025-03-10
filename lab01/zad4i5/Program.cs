using System;
using System.IO;
using System.Text.RegularExpressions;
using Zad5;

class Program{
    static string output = "output.txt";
    static int n = 5;
    static int[] range = {0, 10};
    static int seed = 42;
    static string type = "float";

    static bool check_args(string[] args){
        output = args[0];

        try {
            n = int.Parse(args[1]);
        }
        catch (Exception e) {
            Console.WriteLine("Error: Zły format liczby n-ilość wygenerowanych liczb. " + e.Message);
            return false;
        }

        string pattern = @"^(\[|\()(\d+),(\d+)(\]|\))$";
        Match match = Regex.Match(args[2], pattern);
        if (!match.Success){
            Console.WriteLine("Error: Niepoprawny format przedziału. Oczekiwany format np. [0,10) lub [3,7)");
            return false;
        }

        range[0] = int.Parse(match.Groups[2].Value);
        range[1] = int.Parse(match.Groups[3].Value);

        try {
            seed = int.Parse(args[3]);
        }
        catch (Exception e) {
            Console.WriteLine("Error: Zły format wartości seed. " + e.Message);
            return false;
        }

        type = args[4].ToLower();
        if (type != "int" && type != "float") {
            Console.WriteLine("Error: Ostatni argument musi być 'int' lub 'float'.");
            return false;
        }

        return true;
    }

    static void Main(string[] args){
        if (args.Length < 5){
            Console.WriteLine(@"Error: Nie podano wystarczającej liczby argumentów. 
            Template: 
            dotnet run 
            output.txt (nazwa pliku wynikowego)
            5 (n-ilość wygenerowanych liczb) 
            [0,10) (przedział dla wartości losowych liczb)
            42 (seed)
            int (int/float int-całkowite, float-rzeczywiste)");
            return;
        }

        if (check_args(args)){
            Random random = new Random(seed);
            using (StreamWriter sw = new StreamWriter(output, append: false)){
                if (type == "int"){
                    for (int i = 0; i < n; i++){
                        sw.WriteLine(random.Next(range[0], range[1]));
                    }
                }
                else {
                    for (int i = 0; i < n; i++){
                        sw.WriteLine(random.NextDouble() * (range[1] - range[0]) + range[0]);
                    }
                }
            }
        }

        Measures zad5 = new Measures(output);
        zad5.Main();
   }
}