using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_PO_KW.Models
{
    public class Dostepnosc
    {
        public int IdGodziny { get; set; }
        public int IdWeterynarz { get; set; }
        public int DzienTygodnia { get; set; } 
        public TimeSpan GodzStart { get; set; }
        public TimeSpan GodzKoniec { get; set; }
        public string NazwaDnia => DzienTygodnia switch
        {
            1 => "Poniedziałek",
            2 => "Wtorek",
            3 => "Środa",
            4 => "Czwartek",
            5 => "Piątek",
            6 => "Sobota",
            _ => throw new ArgumentOutOfRangeException("Nieprawidłowy dzień tygodnia")
        };
    }
}

