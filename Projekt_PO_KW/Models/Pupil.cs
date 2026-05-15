using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_PO_KW.Models
{
    public class Pupil
    {
        public int IdPupil { get; set; }
        public string Imie { get; set; } = string.Empty;
        public string Gatunek { get; set; } = string.Empty;
        public int Wiek { get; set; }
        public string Rasa { get; set; } = string.Empty;
        public string Plec { get; set; } = string.Empty;
    }
}


