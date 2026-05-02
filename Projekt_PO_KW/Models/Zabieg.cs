using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_PO_KW.Models
{
    public class Zabieg
    {
        public int IdZabieg { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public string? Opis { get; set; }
        public decimal Cena { get; set; }
        public int CzasTrwaniaMin { get; set; }
        public string Nazwa_Cena => $"{Nazwa} ({Cena:C})";
    }
}
