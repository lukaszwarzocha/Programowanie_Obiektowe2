using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Labolatorium3.Zadanie1a;

namespace Labolatorium3.Zadanie1b
{
    internal class Reader : Person
    {
        private List<Book> przeczytane_ksiazki;

        public Reader(string FirstName, string LastName, int wiek) : base(FirstName,LastName,wiek)
        {
           przeczytane_ksiazki = new List<Book>();
        }

        public void AddBook(Book book)
        {
            przeczytane_ksiazki.Add(book);
        }

        public void ViewBook()
        {
            Console.WriteLine("Ksiazki przeczytane przez: ");
            base.View();
            Console.WriteLine("\n Tytuły to: ");

            foreach (var ksiazka in przeczytane_ksiazki)
            {
                ksiazka.View();
            }
        }
    }
}
