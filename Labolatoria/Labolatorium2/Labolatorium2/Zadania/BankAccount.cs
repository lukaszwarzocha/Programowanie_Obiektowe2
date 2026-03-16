using System;
using System.Collections.Generic;
using System.Text;

namespace Labolatorium2.Zadania
{
    internal class BankAccount
    {
        public string Wlasciciel { get; set; }
        public double Saldo { get; private set; }

        public BankAccount(string wlasciciel, double saldo)
        {
            Wlasciciel = wlasciciel;
            Saldo = saldo;
        }

        public void Wplata(double Saldo)
        {
            if (Saldo > 0)
            {
                this.Saldo += Saldo;
            }
        }

        public void Wyplata(double Saldo)
        {
            if (Saldo <= this.Saldo)
            {
                this.Saldo -= Saldo;
            }
            else
            {
                Console.Write("Brak wystarczajacych srodkow na koncie!");
            }
        }

        public void StanKonta(string klient)
        {
            Console.WriteLine("Klient o nazwie "+this.Wlasciciel+", posiada saldo w wysokosci "+Saldo+".");
        }
    }
}
