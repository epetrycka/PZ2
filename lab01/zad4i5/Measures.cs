using System.IO;

namespace Zad5
{
    public class Measures
    {
        public string file_name;

        public Measures(string file_name)
        {
            this.file_name = file_name;
        }

        public void Main()
        {
            int line_counter = 0;
            int nums_counter = 0;
            int signs_counter = 0;
            float max_num = float.NegativeInfinity;
            float min_num = float.PositiveInfinity;
            float sum = 0;

            if (!File.Exists(file_name)){
                Console.WriteLine("Error: Plik nie istnieje.");
            }
            else{
                using (StreamReader sr = new StreamReader(file_name)){
                    while (!sr.EndOfStream){
                        line_counter += 1;
                        string? line = sr.ReadLine();
                        if (string.IsNullOrEmpty(line)){ continue; }
                        signs_counter += line.Length;
                        float number;
                        float.TryParse(line, out number);
                        nums_counter += 1;
                        sum += number;
                        if (max_num < number){
                            max_num = number;
                        }
                        if (min_num > number){
                            min_num = number;
                        }
                    }
                }
                Console.WriteLine("Wyniki danych (zadanie5): ");
                Console.WriteLine($"Ilość linii w pliku: {line_counter}");
                Console.WriteLine($"Ilość znaków w pliku: {signs_counter}");
                Console.WriteLine($"Najwięskza liczba w pliku: {max_num}");
                Console.WriteLine($"Najmniejsza liczba w pliku: {min_num}");
                Console.WriteLine($"Średnia liczb w pliku: {sum/nums_counter}");
            }
        }
    }
}
