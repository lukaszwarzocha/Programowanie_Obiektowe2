using System;
using System.Collections.Generic;
using System.Text;

namespace Labolatorium3.Zadanie1a
{
    internal class Person
    {
        protected String FirstName;
        protected String LastName;
        protected int wiek;


        public Person(String FirstName, String LastName, int wiek)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.wiek = wiek;  
        }

        public void View()
        {
            Console.Write($"{FirstName} {LastName} (wiek: {wiek})");
        }
    }
}
