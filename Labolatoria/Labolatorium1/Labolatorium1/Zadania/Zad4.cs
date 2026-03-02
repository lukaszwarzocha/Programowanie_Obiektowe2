using System;
using System.Collections.Generic;
using System.Text;

namespace Labolatorium1.Zadania
{
    internal class Zad4
    {

        public void Run()
        {
            while (true)
            { 
                Console.WriteLine("Podaj liczbe całkowitą lub ujemną aby zakończyć!");
                int liczba = Convert.ToInt32(Console.ReadLine());

                if (liczba < 0) { break; }
            }
        }
    }
}
