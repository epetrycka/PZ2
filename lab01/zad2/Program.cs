using System;

namespace zad2;

class Program{
    static void Main(string[] args){
        Console.WriteLine("Wypisz dane liczbowe");
        int value = 1;
        do {
            try{
                value = int.Parse(Console.ReadLine());
            }
            catch (Exception e){}
        } while (value!=0);
    }
}