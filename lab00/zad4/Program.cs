using System;
using System.Collections.Generic;

namespace zad4;

class Program{
    static void Main(string[] args){
        List<string> tones = new List<string>{"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "B", "H"};
        List<int> dur = new List<int>{2, 2, 1, 2, 2, 2, 1};
        int len = tones.Count;
        Console.WriteLine("Podaj dźwięk podstawowy: ");
        string? inital = Console.ReadLine();
        if (string.IsNullOrEmpty(inital) || !tones.Contains(inital)) { return; }
        int start_index = tones.IndexOf(inital);
        List<string> gama = new List<string>();
        gama.Add(inital);
        foreach (int val in dur){
            start_index = (start_index + val)%len;
            gama.Add(tones[start_index]);
        }
        Console.WriteLine($"Gama dur: {string.Join(" ", gama)}");
    }
}