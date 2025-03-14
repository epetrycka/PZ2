using System;
using System.IO;

namespace lab2;

public class OsobaPrawna : PosiadaczRachunku{
    private string? nazwa;
    private string? siedziba;

    public OsobaPrawna(string nazwa, string siedziba){
        Nazwa = nazwa ?? throw new ArgumentNullException(nameof(nazwa), "Nazwa nie może być null");
        Siedziba = siedziba ?? throw new ArgumentNullException(nameof(siedziba), "Siedziba nie może być null");
    }

    public string Nazwa {get;}
    public string Siedziba {get;}

    public override string ToString(){
        return $"Osoba prawna: {Nazwa}, Siedziba: {Siedziba}";
    }
}