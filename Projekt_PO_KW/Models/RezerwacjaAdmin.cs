using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_PO_KW.Models
{
    public class RezerwacjaAdmin
    {
        public int IdRezerwacja { get; set; }
        public int IdWeterynarz { get; set; }
        public DateTime DataRezerwacji { get; set; }
        public TimeSpan GodzStart { get; set; }
        public TimeSpan GodzKoniec { get; set; }
        public string Status { get; set; } = string.Empty;

        public string ImieNazwiskoWeterynarza { get; set; } = string.Empty;
        public string ImiePupila { get; set; } = string.Empty;
        public string NazwaZabiegu { get; set; } = string.Empty;
        public string ImieNazwiskoUzytkownika { get; set; } = string.Empty;
    }
}