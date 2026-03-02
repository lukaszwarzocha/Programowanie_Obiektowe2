using System;
using System.Collections.Generic;
using System.Text;

namespace Labolatorium1.Zadania
{
    internal class Zad1
    {
        public void Run()
        {
            Console.WriteLine("Podaj a: ");
            double a = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Podaj b: ");
            double b = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Podaj c: ");
            double c = Convert.ToDouble(Console.ReadLine());

            delta(a, b, c);
        }

        private void delta(double a, double b, double c)
        {
            double delta = (b * b) - 4 * a * c;
            double pierwiastek = Math.Sqrt(delta);

            if (delta == 0)
            {
                double x0 = -pierwiastek / 4 * a;
                Console.WriteLine($"Delta rowna 0, jedno rozwiazanie ktore jest rowne {x0:F2}");
            }
            else if (delta > 0)
            {
                double x1 = (-b - pierwiastek) / (2 * a);
                double x2 = (-b + pierwiastek) / (2 * a);
                Console.WriteLine($"Delta dodatnia, dwa rozwiazania x1 = {x1:F2} i x2 = {x2:F2}");
            }
            else { Console.WriteLine("Brak rozwiazania, delta ujemna"); }
        }
    }
}
