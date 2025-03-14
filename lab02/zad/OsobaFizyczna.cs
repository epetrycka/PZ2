using System;
using System.Linq;

namespace lab2;

public class OsobaFizyczna : PosiadaczRachunku
{
    private string? pesel;

    public OsobaFizyczna(string? imie, string? nazwisko, string? drugieImie, string? PESEL, string? numerPaszportu)
    {
        if (string.IsNullOrEmpty(PESEL) && string.IsNullOrEmpty(numerPaszportu))
        {
            throw new Exception("PESEL albo numer paszportu muszą być nie null");
        }

        Imie = imie ?? throw new ArgumentNullException(nameof(imie), "Pole imie nie może być null");
        Nazwisko = nazwisko ?? throw new ArgumentNullException(nameof(nazwisko), "Pole nazwisko nie może być null");
        DrugieImie = drugieImie;

        if (!ValidatePesel(PESEL))
        {
            throw new ArgumentException("Zły format pola PESEL");
        }

        pesel = PESEL;
        NumerPaszportu = numerPaszportu;
    }

    public string Imie { get; }
    public string Nazwisko { get; }
    public string? DrugieImie { get; private set; }
    
    public string? Pesel
    {
        get => pesel;
        private set
        {
            if (!ValidatePesel(value))
            {
                throw new ArgumentException("Zły format pola PESEL");
            }
            pesel = value;
        }
    }

    public string? NumerPaszportu { get; private set; }

    public override string ToString()
    {
        return $"Osoba fizyczna: {Imie} {Nazwisko}";
    }

    public static bool ValidatePesel(string? pesel)
    {
        if (string.IsNullOrEmpty(pesel))
        {
            return false;
        }

        if (pesel.Length != 11)
        {
            return false;
        }

        return pesel.All(char.IsDigit);
    }
}