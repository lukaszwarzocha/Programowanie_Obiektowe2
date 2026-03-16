using Labolatorium2;
using Labolatorium2.Zadania;

Console.WriteLine("Labolatorium 2 - Zadania\n\n");
Console.WriteLine("Zadanie 1 - Klasa Person\n");

Person osoba = new Person("Kacper", "Zoła", 22);
osoba.Wyswietlinformacje();

Console.WriteLine("\n\nZadanie 2 - Klasa BankAccount\n");
BankAccount bank = new BankAccount("Kacper Zoła", 0);
bank.Wplata(20000);
bank.StanKonta("Kacper Zoła");
bank.Wyplata(500);
bank.StanKonta("Kacper Zoła");

Console.WriteLine("\n\nZadanie 3 - Klasa Student\n");   
Student student = new Student("Kacper", "Zoła");
student.DodajOcene(2);
student.DodajOcene(3);
student.DodajOcene(4);
student.DodajOcene(5);
student.DodajOcene(6);
student.SredniaOcen();

Console.WriteLine("\n\nZadanie 4 - Klasa Licz\n");
Licz licz = new Licz(2);
licz.Dodaj(5);
licz.WyswietlLiczbe();
licz.Odejmij(8);
licz.WyswietlLiczbe();

Console.WriteLine("\n\nZadanie 5 - Klasa Sumator\n");
Sumator sumator = new Sumator(new int[] { 1, 2, 3, 4, 5 });
sumator.WyswietlZawartosc();
sumator.IleElementow();
sumator.Suma();
sumator.SumaPodziel2();


Console.WriteLine("\n\nNaciśnij dowolny klawisz, aby zakończyć program!");
Console.ReadKey();

