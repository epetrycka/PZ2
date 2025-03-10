using System;
using System.Collections.Generic;

namespace zadanie3;

class Program{
    static void Main(string[] args){
        if (args.Length == 0){
            Console.WriteLine("Error: Nie podano nazwy pliku tekstowego.");
            return;
        }
        else if (args.Length >= 2) {
            Console.WriteLine("Error: Podano za dużo argumentów. Podaj tylko nazwę pliku.");
            return;
        }
        string file_name = args[0]; 
        Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
        int line_count = 0;
        int max_val = -1;
        int value;
        
        using (StreamReader sr = new StreamReader(file_name)){
            while(!sr.EndOfStream){
                line_count += 1; 
                string? line = sr.ReadLine();
                if (string.IsNullOrEmpty(line)) { continue; }
                try{
                    value = int.Parse(line);
                }
                catch (Exception) { continue; }
                if (!dict.ContainsKey(value)){
                    dict[value] = new List<int>();
                }
                dict[value].Add(line_count);
                if (max_val == -1){
                    max_val = value;
                }
                else {
                    if (max_val < value){
                        max_val = value;
                    }
                }
            }
        }
        if (dict.Count == 0){
            Console.WriteLine("Error: Plik jest pusty");
            return;
        }
        Console.WriteLine($"{max_val}, linijki: {string.Join(", ", dict[max_val])}");
    }
}