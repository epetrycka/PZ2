using System;
using System.IO;

class Program{
    static void Main(string[] args){
        Console.WriteLine("Wypisz dane liczbowe, a na końcu napisz 0:");
        int result = 0;
        int count = 0;

        while(true) {
            string? input = Console.ReadLine();
            if (input == null) continue;

            if (int.TryParse(input, out int value)){
                if (value == 0) break;
                count++;
                result += value;
            }
        }

        float mean = (count > 0) ? (float)result / count : 0;
        Console.WriteLine("Średnia zapisana do pliku 'wynik2.txt': " + mean);

        using (StreamWriter sw = new StreamWriter("wynik2.txt", append: true)) {
            sw.WriteLine("Średnia z zadania 2: " + mean);
        }
    }
}
