using Labolatorium3.Zadanie1a;
using Labolatorium3.Zadanie1b;

Person person = new Person("Michał", "Kowalski", 45);
Book book1 = new Book("W akademiku", person, new DateTime(2000,05,15));

book1.View();


Console.WriteLine("----------------------------------------");

Book book2 = new Book("Akademik1", person, new DateTime(2000, 05, 15));
Book book3 = new Book("Akademik2", person, new DateTime(2005, 05, 15));
Book book4 = new Book("Akademik1", person, new DateTime(2010, 05, 15));

Reader r1 = new Reader("Maciek", "Kania", 24);
Reader r2 = new Reader("Kacper", "Zoła", 22);
r1.AddBook(book2);
r1.AddBook(book4);
r2.AddBook(book3);


r1.ViewBook();
r2.ViewBook();


Console.WriteLine("----------------------------------------");

Console.WriteLine("Press any key!");

