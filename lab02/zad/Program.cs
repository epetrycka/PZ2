using System;
using System.Collections.Generic;
using lab2;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Model rachunku bankowego ===\n");

        PosiadaczRachunku osoba1 = new OsobaPrawna("Firma ABC", "Kraków");
        PosiadaczRachunku osoba2 = new OsobaPrawna("Firma XYZ", "Warszawa");

        List<PosiadaczRachunku> listaPosiadaczy1 = new() { osoba1 };
        List<PosiadaczRachunku> listaPosiadaczy2 = new() { osoba2 };

        RachunekBankowy rachunek1 = new("123-456-789", 5000, false, listaPosiadaczy1);
        RachunekBankowy rachunek2 = new("987-654-321", 2000, true, listaPosiadaczy2);

        Console.WriteLine($"Utworzono rachunek: {rachunek1.Numer} z saldem: {rachunek1.StanRachunku} zł");
        Console.WriteLine($"Utworzono rachunek: {rachunek2.Numer} z saldem: {rachunek2.StanRachunku} zł\n");

        try
        {
            Console.WriteLine("Próba przelania 1000 zł z rachunku1 do rachunku2...");
            RachunekBankowy.DokonajTransakcji(rachunek1, rachunek2, 1000, "Opłata za usługę");
            Console.WriteLine("Transakcja zakończona sukcesem!");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Błąd: {e.Message}");
        }

        Console.WriteLine($"Saldo rachunku1: {rachunek1.StanRachunku} zł");
        Console.WriteLine($"Saldo rachunku2: {rachunek2.StanRachunku} zł\n");

        try
        {
            Console.WriteLine("Próba przelania 10000 zł z rachunku1 do rachunku2...");
            RachunekBankowy.DokonajTransakcji(rachunek1, rachunek2, 10000, "Duża transakcja");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Błąd: {e.Message}");
        }

        Console.WriteLine("\n=== Historia transakcji rachunku1 ===");
        foreach (var transakcja in rachunek1.Transakcje)
        {
            Console.WriteLine($"{transakcja.Opis}: {transakcja.Kwota} zł");
        }

        Console.WriteLine("\n=== Historia transakcji rachunku2 ===");
        foreach (var transakcja in rachunek2.Transakcje)
        {
            Console.WriteLine($"{transakcja.Opis}: {transakcja.Kwota} zł");
        }

        Console.WriteLine("\n=== Koniec testów ===");
    }
}
