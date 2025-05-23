# Laboratorium 07.5: Zastosowanie kryptografii i programowania sieciowego - komunikator z szyfrowaniem połączenia
## Programowanie zaawansowane 2

- Maksymalna liczba punktów: 10

- Skala ocen za punkty:
    - 9-10 ~ bardzo dobry (5.0)
    - 8 ~ plus dobry (4.5)
    - 7 ~ dobry (4.0)
    - 6 ~ plus dostateczny (3.5)
    - 5 ~ dostateczny (3.0)
    - 0-4 ~ niedostateczny (2.0)

Celem laboratorium jest zaimplementowanie komunikatora o architekturze klient-serwer, który będzie wykorzystywał kryptografię klucza symetrycznego i asymetrycznego w celu szyfrowania połączenia. Programy powinny mieć konsolowy interfejs użytkownika pozwalający na przetestowanie ich działania. Jeśli jakieś polecenie może być odczytane wieloznacznie lub coś nie jest w nim sprecyzowane, proszę zinterpretować je według uznania.

1. [5 punktów] Napisz serwer, który będzie obsługiwał połączenia wielu programów klienckich przy użyciu protokołu TCP/IP z obsługą połączenie full duplex. Napisz program kliencki, który będzie mógł łączyć się z tym serwerem i obsługiwał połączenie full duplex. Zadaniem serwera jest przekazywanie wiadomości pomiędzy połączonymi z nim klientami. Klient za pośrednictwem serwera ma mieć możliwość przesłania wiadomości do innego klienta, który w danej chwili jest połączony z serwerem. Zaprojektuj i zaimplementuj protokół komunikacyjny działający w oparciu o protokół TCP/IP który:
- Pozwoli na połączenie klienta z serwerem i przesłanie przez klienta unikalnej nazwy klienta (jeśli nazwa się powtarza, czyli klient o takiej nazwie już jest, serwer ma zakończyć połączenie). Nazwa klienta jest umowna i deklarowana przez klienta, który się łączy, może to być na przykład "Stasiu", "Marysia" i "Massacrator657" itp. Serwer ma pamiętać tą nazwę tak długo, jak długo klient jest z nim połączony.
- Pozwoli na sprawdzenie z poziomu klienta, czy z serwerem jest połączony klient o konkretnej nazwie, na przykład "Stasiu" może odpytać serwer, czy jest z nim w tej chwili połączony "Massacrator657"
- Pozwoli na przesłanie wiadomości tekstowej za pośrednictwem serwera pomiędzy klientami, na przykład "Marysia" może wysłać wiadomość (na przykład wpisaną za pośrednictwem klawiatury) do "Stasia" a program klienta "Stasia" ma ją odczytać i wyświetlić.
2. [5 punkty] Zaimplementuj szyfrowanie połączenia. 
- W momencie pierwszego połączenia pomiędzy dwoma klientami serwer ma wysłać do każdego z nich nowy klucz symetryczny, który będzie służył do szyfrowania połączenia pomiędzy nimi. Od tego momentu komunikacja pomiędzy klientami ma być szyfrowana tym kluczem, szyfrowanie i odszyfrowywanie wiadomości ma odbywać się po stronie klientów.

### UWAGA!

Warto jest zaprojektować protokół w taki sposób, aby np. wiadomości składały się z ramek następującej postaci
|typ wiadomości|długość wiadomości|dane|

typ wiadomości - wartość liczbowa określająca co klient chce zrealizować np. nawiązywanie połączenia z serwerem, sprawdzenie statusu połączenia innego klienta itp.
długość wiadomości - długość wiadomości w bajtach
dane - dane które są przesyłane, interpretuje się je w zależności od typu wiadomości i może to być np. wiadomość tekstowa, nazwa klienta który inicjuje połączenie z serwerem, nazwa klient o którego status połączenia z serwerem chce określić klient.

## ❤ Zadanie na dodatkową 5

Jeśli wykonasz wszystkie podpunkty z zadania 1 i 2 stwórz nowy projekt (aby nie nadpisać starego), w którym zostanie zmodyfikowany sposób szyfrowania połączenia z zadań 1 i 2.
- Każdy z klientów ma mieć własną parę klucz prywatny - klucz publiczny. Przy pierwszym połączeniu z serwerem klient przesyła na serwer swój klucz publiczny. 
- W momencie inicjowania połączenia z drugim klientem, klient pobiera z serwera klucz publiczny drugiego klienta, tworzy losowy klucz symetryczny, szyfruje ten klucz symetryczny kluczem publicznym drugiego klienta, przesyła za pomocą serwera ten zaszyfrowany klucz symetryczny do drugiego klienta. Drugi klient odszyfrowuje klucz i od tego momentu będą używali tak uzgodnionego klucza symetrycznego do szyfrowania połączeń z tym pierwszym klientem.

Przykład: 
- "Stasiu" łączy się po raz pierwszy, z serwerem, więc przesyła mu swój klucz publiczny. 
- "Stasiu" chce napisać wiadomość tekstową do "Massacrator657". "Massacrator657" jest już połączony z serwerem "Stasiu" pisze do niego po raz pierwszy.  Z tego powodu "Stasiu" pobiera klucz publiczny "Massacrator657" z serwera, generuje losowy klucz symetryczny, szyfruje kluczem publicznym "Massacrator657" i przesyła go za pośrednictwem serwera do "Massacrator657". Następnie wysyła do "Massacrator657" swoją wiadomość tekstową zaszyfrowaną kluczem symetrycznym.
- "Massacrator657" odbiera wiadomość. Deszyfruje ją swoim kluczem prywatnym, odbierając w ten sposób klucz symetryczny. Następnie odbiera od "Stasia" zaszyfrowaną kluczem symetrycznym wiadomość, którą deszyfruje.
- Jeżeli "Stasiu" i "Marysia" będą chcieli się komunikować, użyją innego losowego klucza symetrycznego do szyfrowania niż ten, który wykorzystywany jest pomiędzy "Stasiem" a "Massacrator657".

Samodzielnie rozszerz zaprojektowany w punktach 1 i 2 protokół połączeniowy tak, aby te funkcjonalności były możliwe.

### Pamiętaj! 

Nigdy nie przesyłaj klucza prywatnego przez sieć! Ma on być umieszczony lokalnie na komputerze klienta, który jest jego posiadaczem.

## ❤❤ Zadanie na jeszcze jedną dodatkową 5

Jeśli wykonasz wszystkie podpunkty z zadania 1 i 2 oraz zadanie ❤ stwórz nowy projekt  (aby nie nadpisać starego). Dodaj do projektu zrealizowanego w ❤ funkcjonalność polegającą na tym, że każda wiadomość pomiędzy klientami będzie podpisywana ich kluczem publicznym a następnie prawidłowość podpisu będzie weryfikowana po stronie drugiego klienta.

Samodzielnie rozszerz zaprojektowany w punktach 1 i 2 oraz ❤ protokół połączeniowy tak, aby te funkcjonalności były możliwe.

## ❦ Powodzenia! ❧
