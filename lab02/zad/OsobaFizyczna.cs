using System;
using System.IO;

namespace lab2;

public class OsobaFizyczna : PosiadaczRachunku{
    private string? imie;
    private string? nazwisko;
    private string? drugieImie;
    private string? PESEL;
    private string? numerPaszportu;

    public OsobaFizyczna (string? imie, string? nazwisko, string? drugieImie, string? PESEL, string? numerPaszportu){
        if (string.IsNullOrEmpty(PESEL) && string.IsNullOrEmpty(numerPaszportu)){
            throw new Exception("PESEL albo numer paszportu muszą być nie null");
        }

        Imie = imie ?? throw new ArgumentNullException(nameof(imie), "Pole imie nie może być null");
        Nazwisko = nazwisko ?? throw new ArgumentNullException(nameof(nazwisko), "Pole nazwisko nie może być null");
        DrugieImie = drugieImie;
        Pesel = PESEL;
        NumerPaszportu = numerPaszportu;
    }

    public string Imie {get;}
    public string Nazwisko {get;}
    public string? DrugieImie {get; private set;}
    public string? Pesel {get; private set;}
    public string? NumerPaszportu {get; private set;}


    public override string ToString(){
        return $"Osoba fizyczna: {Imie} {Nazwisko}";
    }
}