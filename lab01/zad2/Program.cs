using System;
using System.Collections.Generic;

namespace zad2;

class Program{
    static void Main(string[] args){
        List<string> text = new List<string>();
        while(true){
            if (line == "koniec!"){ break; }
            string? line = Console.ReadLine();
            if (string.IsNullOrEmpty(line)) { continue; }
            text.Add(line);
        }
        text.Sort();
        using (StreamWriter sw = new StreamWriter("wyniki2.txt", append:true)){
            foreach (string word in text){
                sw.WriteLine(word);
            }
        }
    }
}