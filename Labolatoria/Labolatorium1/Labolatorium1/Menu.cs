using Labolatorium1.Zadania;
using System;
using System.Collections.Generic;
using System.Text;

namespace Labolatorium1
{
    internal class Menu
    {

        public void Run()
        {

            Console.WriteLine("Podaj numer zadania (1-5):");
            int zadanie = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"Wybrałeś zadanie numer {zadanie}:\n");
            wybor(zadanie);


        }
        private void wybor(int zadanie)
        {
            switch (zadanie)
            {
                case 1:
                    Zad1 zadanie1 = new Zad1();
                    zadanie1.Run();
                    break;
                case 2:
                    Zad2 zadanie2 = new Zad2();
                    zadanie2.Run();
                    break;
                case 3:
                    Zad3 zadanie3 = new Zad3();
                    zadanie3.Run();
                    break;
                case 4:
                    Zad4 zadanie4 = new Zad4();
                    zadanie4.Run();
                    break;
                case 5:
                    Zad5 zadanie5 = new Zad5();
                    zadanie5.Run();
                    break;
                default:
                    Console.WriteLine("Nie znaleziono zadania!");
                    break;

            }
        }

    }
}
