using System;
using System.Collections.Generic;
using System.Text;

namespace Labolatorium2.Zadania
{
    internal class Licz
    {
        private int value = 1;

        public Licz(int value)
        {
            this.value = value; 
        }

        public void Dodaj(int v)
        {
                this.value += v;
        }

        public void Odejmij(int v)
        {
            this.value -= v;
        }
       
        public void WyswietlLiczbe()
        {
            Console.WriteLine("Liczba wynosi: " + value);
        }
    }
}
