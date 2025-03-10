using System;
using System.Collections.Generic;
using System.IO;

namespace zadanie3
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Error: Nie podano nazwy pliku tekstowego lub szukanego tekstu.");
                return;
            }

            string file_name = args[0];
            string find_text = string.Join(" ", args[1..]);
            if (string.IsNullOrEmpty(find_text)){
                Console.WriteLine("Error: nie podano szukanego tekstu");
                return;
            }
            int line_count = 0;
            Dictionary<int, List<int>> lines = new Dictionary<int, List<int>>();

            try
            {
                using (StreamReader sr = new StreamReader(file_name))
                {
                    while (!sr.EndOfStream)
                    {
                        line_count++;
                        string? line = sr.ReadLine();
                        if (string.IsNullOrEmpty(line)){ continue; }
                        int position = line.IndexOf(find_text);
                        while (position != -1)
                        {
                            if (!lines.ContainsKey(line_count))
                                lines[line_count] = new List<int>();

                            lines[line_count].Add(position);
                            position = line.IndexOf(find_text, position + 1);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Błąd przy odczycie pliku: {e.Message}");
                return;
            }

            if (lines.Count == 0)
            {
                Console.WriteLine("Error: Nie znaleziono napisu");
                return;
            }
            Console.WriteLine($"Wystąpienia tekstu {find_text} w pliku {file_name}:");
            foreach (var entry in lines)
            {
                Console.WriteLine($"linia {entry.Key}, pozycje: {string.Join(", ", entry.Value)}");
            }
        }
    }
}