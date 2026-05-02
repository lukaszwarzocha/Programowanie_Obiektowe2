using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_PO_KW.Models
{
    public class Rezerwacja
    {
        public int IdRezerwacja { get; set; }
        public int IdWeterynarz { get; set; }
        public int IdPupil { get; set; }
        public int IdZabieg { get; set; }
        public int IdUzytkownik { get; set; }
        public DateTime DataRezerwacji { get; set; }
        public TimeSpan Godzina_Start { get; set; }
        public TimeSpan Godzina_Koniec { get; set; }
        public string Status { get; set; } = "Zarezerwowany";
    }
}
