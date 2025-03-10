using System;
using System.IO;
using System.Collections.Generic;

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