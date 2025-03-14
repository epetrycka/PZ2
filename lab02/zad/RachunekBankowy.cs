using System;
using System.IO;
using System.Collections.Generic;

namespace lab2;

public class RachunekBankowy{
    private string? numer;
    private Decimal stanRachunku;
    private bool czyDozwolonyDebet = false;
    private List<PosiadaczRachunku> posiadaczeRachunku = new();
    private List<Transakcja> transakcje = new ();

    public string Numer {get; private set;}
    public Decimal StanRachunku {get; private set;}
    public bool CzyDozwolonyDebet {get ; private set;}
    public IReadOnlyList<PosiadaczRachunku> PosiadaczeRachunku => posiadaczeRachunku.AsReadOnly();
    public IReadOnlyList<Transakcja> Transakcje => transakcje.AsReadOnly();

    public RachunekBankowy(string? numer, Decimal? stanRachunku, bool czyDozwolonyDebet, List<PosiadaczRachunku> posiadaczeRachunku){
        if (posiadaczeRachunku.Count < 1){
            throw new Exception("Rachunek musi mieć co najmniej jednego posiadacza");
        }

        Numer = numer ?? throw new ArgumentNullException(nameof(numer), "Pole numer nie może być null");
        StanRachunku = stanRachunku ?? throw new ArgumentNullException(nameof(stanRachunku), "Pole stan rachunku nie może być null");
        CzyDozwolonyDebet = czyDozwolonyDebet;
        this.posiadaczeRachunku = posiadaczeRachunku;
    }

    public static void DokonajTransakcji(RachunekBankowy rachunekZrodlowy, RachunekBankowy rachunekDocelowy, decimal kwota, string opis){
        if (kwota <= 0){
            throw new Exception("Kwota transakcji nie może być ujemna ani równa 0");
        }
        if (rachunekZrodlowy == null && rachunekDocelowy == null){
            throw new Exception("Brak podanego rachunku zrodlowego lub docelowego");
        }
        if (rachunekZrodlowy != null && !rachunekZrodlowy.CzyDozwolonyDebet && kwota > rachunekZrodlowy.StanRachunku){
            throw new Exception("Brak możliwości dokonania transakcji, sprawdź stan rachunku");
        }
        
        if (rachunekZrodlowy == null){
            rachunekDocelowy.StanRachunku += kwota;
            rachunekDocelowy.transakcje.Add(new Transakcja(null, rachunekDocelowy, kwota, opis));
        }else if (rachunekDocelowy == null){
            rachunekZrodlowy.StanRachunku -= kwota;
            rachunekZrodlowy.transakcje.Add(new Transakcja(rachunekZrodlowy, null, kwota, opis));
        }else {
            rachunekDocelowy.StanRachunku += kwota;
            rachunekDocelowy.transakcje.Add(new Transakcja(rachunekZrodlowy, rachunekDocelowy, kwota, opis));
            rachunekZrodlowy.StanRachunku -= kwota;
            rachunekZrodlowy.transakcje.Add(new Transakcja(rachunekZrodlowy, rachunekDocelowy, kwota, opis));
        }
    }
}