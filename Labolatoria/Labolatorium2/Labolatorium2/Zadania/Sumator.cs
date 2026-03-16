using System;
using System.Collections.Generic;
using System.Text;


// do dokonczenia

namespace Labolatorium2.Zadania
{
    internal class Sumator
    {
        private int[] Liczby;

        public Sumator(int[] Liczby)
        {
            this.Liczby = Liczby;
        }

        public void Suma()
        {
            if (Liczby.Length == 0)
            {
                Console.WriteLine("Tablica liczb jest pusta!");
            }

            int suma = 0;
            foreach (int liczba in Liczby)
            {
                suma += liczba;
            }
            Console.WriteLine("Suma liczb w tablicy wynosi: " + suma);
        }

        public void SumaPodziel2()
        {
            if (Liczby.Length == 0)
            {
                Console.WriteLine("Tablica liczb jest pusta!");
            }

            int suma_podzielne_przez_2 = 0;

            foreach (int liczba in Liczby)
            {
                if (liczba % 2 == 0)
                {
                    suma_podzielne_przez_2 += liczba;
                }
            }
            Console.WriteLine("Suma liczb podzielnych przez 2 w tablicy wynosi: " + suma_podzielne_przez_2);
        }

        public void IleElementow()
        {
            if (Liczby.Length == 0)
            {
                Console.WriteLine("Tablica liczb jest pusta!");
            }

            Console.WriteLine("\nLiczba elementow w tablicy: " + Liczby.Length);
        }

        public void WyswietlZawartosc()
        {
            if (Liczby.Length == 0)
            {
                Console.WriteLine("Tablica liczb jest pusta!");
            }

            Console.Write("Zawartosc tablicy: ");
            foreach (int liczba in Liczby)
            {
                Console.Write(liczba +", ");
            }
        }


    }
}
