using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Labolatorium2.Zadania
{
    internal class Person
    {
        private string firstname;
        private string lastname;
        private int age;

        public Person(string firstname, string lastname, int age) {
            if (firstname.Length < 2 || lastname.Length < 2)
            {
                Console.WriteLine("Imie oraz Nazwisko musza miec co najmniej 2 znaki.");
                return;
            }
            else if (age < 0)
            {
                Console.WriteLine("Wiek nie moze byc ujemny!");
                return;
            }
            else
            {
                this.firstname = firstname;
                this.age = age;
                this.lastname = lastname;
            }

        }
        public void Wyswietlinformacje()
        {
            Console.WriteLine("Osoba o imieniu: "+firstname+", nazwisku: "+lastname+" jest w wieku "+age+"");
        }


    }
}
