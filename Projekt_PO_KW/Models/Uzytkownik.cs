using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace Projekt_PO_KW.Models
{
    public class Uzytkownik
    {
        public int IdUzytkownik {  get; set; }
        public string Imie { get; set; } = string.Empty;
        public string Nazwisko { get; set; } = string.Empty;
        public string? Adres { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Haslo { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string Rola { get; set; } = "Uzytkownik";
    }
}
