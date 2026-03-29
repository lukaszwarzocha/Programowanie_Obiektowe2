using System;
using System.Collections.Generic;
using System.Text;

namespace Labolatorium3.Zadanie1a
{
    internal class Book
    {
        private String title;
        private Person author;
        private DateTime data_wydania;
        
        public Book(String title, Person author, DateTime data_wydania)
        {
            this.title = title;
            this.author = author;
            this.data_wydania = data_wydania;
        }

        public void View()
        {
            Console.WriteLine("Nazwa ksiazki to: " + title + ", autor to: ");
            author.View();
            Console.WriteLine(" a data wydania to: " + data_wydania.ToShortDateString());
        }
    }
}
