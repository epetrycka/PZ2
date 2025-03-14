using System;
using System.IO;

namespace lab2;

public class OsobaPrawna : PosiadaczRachunku{
    private string nazwa;
    private string siedziba;

    public OsobaPrawna(string nazwa, string siedziba){
        Nazwa = nazwa;
        Siedziba = siedziba;
    }

    public string Nazwa {get;}
    public string Siedziba {get;}

    public override string ToString(){
        return $"Osoba prawna: {Nazwa}, Siedziba: {Siedziba}";
    }
}