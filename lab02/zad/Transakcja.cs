using System;
using System.IO;

namespace lab2;

public class Transakcja{
    private RachunekBankowy? rachunekZrodlowy;
    private RachunekBankowy? rachunekDocelowy;
    private Decimal? kwota;
    private string? opis;

    public RachunekBankowy? RachunekZrodlowy {get; private set;}
    public RachunekBankowy? RachunekDocelowy {get; private set;}
    public Decimal? Kwota {get; private set;}
    public string? Opis {get; private set;}

    public Transakcja(RachunekBankowy? rachunekZrodlowy, RachunekBankowy? rachunekDocelowy, Decimal? kwota, string? opis){
        if (rachunekZrodlowy == null || rachunekDocelowy == null)
        {
            throw new Exception("Rachunek zrodlowy i rachunek docelowy nie może mieć wartości null");
        }

        RachunekZrodlowy = rachunekZrodlowy;
        RachunekDocelowy = rachunekDocelowy;
        Kwota = kwota;
        Opis = opis;
    }
}