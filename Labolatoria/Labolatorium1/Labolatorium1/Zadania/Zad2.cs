using System;
using System.Collections.Generic;
using System.Text;

namespace Labolatorium1.Zadania
{
    internal class Zad2
    {
        public void Run()
        {
            int[] liczba = new int[10];
            int suma = 0;
            int iloczyn = 1;
            int numer = 0;

            for (int i = 0; i < 10; i++)
            {
                numer = numer + 1;
                Console.Write($"Podaj liczbe numer {numer}: ");
                String wejscie = Console.ReadLine();

                if (!int.TryParse(wejscie, out liczba[i]))
                {
                    Console.WriteLine("Musisz podać liczbę! Spróbuj ponownie.");
                    numer--;
                    i--;
                    continue;
                }
            }

            int min = liczba[0];
            int max = liczba[0];

            for (int i = 0; i < liczba.Length; i++)
            {
                suma += liczba[i];
                iloczyn *= liczba[i];
                if (liczba[i] < min) { min = liczba[i]; }
                if (liczba[i] > max) { max = liczba[i]; }
            }

            double srednia = (double)suma / liczba.Length;

            Console.WriteLine("====================");
            Console.WriteLine($"Suma liczb wynosi: {suma}");
            Console.WriteLine($"Iloczyn liczb wynosi: {iloczyn}");
            Console.WriteLine($"Minimum to: {min}");
            Console.WriteLine($"Maksimum to: {max}");
            Console.WriteLine($"Srednia tych liczb wynosi: {srednia}");
        }
    }
}


