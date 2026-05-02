using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_PO_KW.Models
{
    public class Weterynarz
    {
        public int IdWeterynarz { get; set; }
        public string Imie { get; set; } = string.Empty;
        public string Nazwisko { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string Specjalizacja { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Haslo { get; set; } = string.Empty;
        public string Imie_Nazwisko => $"{Imie} {Nazwisko}";
    }
}
