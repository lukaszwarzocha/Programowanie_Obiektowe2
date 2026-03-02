using System;
using System.Collections.Generic;
using System.Text;

namespace Labolatorium1.Zadania
{
    internal class Zad5
    {

        public void Run()
        {
      
            double[] array = LosujTabliceDouble(10, 0.0, 100.0);
            Console.WriteLine("Tablica przed sortowaniem:");
            WyswietlTablice(array);
            SortowanieBabelkowe(array);
            Console.WriteLine("============================================");
            Console.WriteLine("Tablica po sortowaniu:");
            WyswietlTablice(array);
        }

        private double[] LosujTabliceDouble(int n, double min, double max)
        {
            var rng = new Random();   // opcjonalnie: nowy Random(seed) dla powtarzalności
            double[] arr = new double[n];
            double zakres = max - min;
            for (int i = 0; i < n; i++)
            {
                // NextDouble() -> [0,1), skalujemy do [min, max] 
                arr[i] = min + rng.NextDouble() * zakres;
            }
            return arr
            ;
        }

        private void SortowanieBabelkowe(double[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = 0; j < arr.Length - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        double temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
        }

        private void WyswietlTablice(double[] arr)
        {
            foreach (double num in arr)
            {
                Console.WriteLine($"{num:F2}");
            }
        }
    }
}
