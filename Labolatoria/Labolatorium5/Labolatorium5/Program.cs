using Labolatorium5.Data;
using Labolatorium5.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    // =========================
    // 1) CONNECTION STRING
    // =========================
    // TODO (student):
    // - Uzupe ł nij poprawny connection string do swojej instancji SQL Server
    // - Wskaz ówka: w SSMS sprawd ź nazw ę serwera(np. "." albo "DESKTOP - XYZ")
    // - Wskaz ówka: Database musi by ć ContactDB
    // Przyk ł ad:
    // "Server=.;Database=ContactDB;Trusted_Connection=True;Encrypt=False;";

    const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ContactDB;Integrated Security=True";

    static void Main()
    {
        // TODO (student):
        // -Utwórz obiekt repozytorium ContactRepository, przekazuj ąc ConnectionString
        // - Wskazówka: var repo = new ContactRepository(ConnectionString);

        var repo = new ContactRepository(ConnectionString);

        // < --usu ń null! po poprawnym utworzeniu repo

        while (true)

        {

            PrintMenu();
            string choice = Console.ReadLine() ?? "";

            try
            {
                switch (choice)
                {
                    case "1":
                        Create(repo);
                        break;
                    case "2":
                        ReadAll(repo);
                        break;
                    case "3":
                        Search(repo);
                        break;
                    case "4":
                        Update(repo);
                        break;
                    case "5":
                        Delete(repo);
                        break;
                    case "6":
                        BulkInsertDemo(repo);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór.");
                        break;
                }
            }

            catch (Exception ex)
            {

                Console.WriteLine("Wyst ąpi ł błą d: " + ex.Message);
            }
        }
    }

    static void PrintMenu()
    {

        Console.WriteLine(" \n=== CONTACT MANAGER (ADO.NET + DAL) ===");
        Console.WriteLine("1) Dodaj kontakt");
        Console.WriteLine("2) Pokaż wszystkie");
        Console.WriteLine("3) Wyszukaj po nazwisku");
        Console.WriteLine("4) Edytuj kontakt");
        Console.WriteLine("5) Usuń kontakt");
        Console.WriteLine("6) Bulk insert (transakcja) - demo");
        Console.WriteLine("0) Wyjście");
        Console.Write("Wybór: ");
    }

    // =========================
    // CREATE
    // =========================

    static void Create(ContactRepository repo)

    {

        // TODO (student):
        // -Utwórz obiekt Contact i pobierz dane od u ż ytkownika
        // -Użyj ReadRequired(...) dla imienia i nazwiska
        // -Użyj ReadOptional(...) dla telefonu i email
        // -Wywołaj repo.Add(contact) i wyświetl Id nowego rekordu
        // Wskazówka:

        var c = new Contact { FirstName = "Patryk", LastName = "Nowak", Phone = "997", Email = "mail.lol" };
        c.FirstName = ReadRequired("Twoje imie: ");
        c.LastName = ReadRequired("Twoje nazwisko: ");
        c.Phone = ReadOptional("Twoj numer telefonu (opcjonalnie): ");
        c.Email = ReadOptional("Twoj adres email (opcjonalnie): ");
        int id = repo.Add(c);
    }

    // =========================

    // READ

    // =========================

    static void ReadAll(ContactRepository repo)

    {

        // TODO (student):
        // -Pobierz listę: repo.GetAll()
        // -Wyświetl nagłówek tabeli
        // -Przejdź poliście i wypisz kontakty (możesz użyć ToString() z modelu Contact)

        var repo2 = repo.GetAll();

        foreach (var kontakt in repo2)
        {
            Console.WriteLine(kontakt.ToString());
        }
    }

    // =========================

    // SEARCH BY LAST NAME

    // =========================

    static void Search(ContactRepository repo)

    {

        // TODO (student):
        // -Pobierz od użytkownika fragment nazwiska
        // -Wywołaj repo.SearchByLastName(fragment)
        // -Wyświetl wyniki

        var repo2 = repo.GetAll();

        Console.WriteLine("Podaj nazwisko");
        var nazwisko = Console.ReadLine();

        var wynik = repo.SearchByLastName(nazwisko);

        foreach (var kontakt in wynik)
        {
            Console.WriteLine(kontakt.ToString()); 
        }

       

    }

    // =========================

    // UPDATE

    // =========================

    static void Update(ContactRepository repo)

    {

        // TODO (student):
        // -Pobierz ID (ReadInt)
        // -Pobierz nowe dane (ReadRequired/ReadOptional)
        // -Utwórz Contact z Id i polami
        // -Wywołaj repo.Update(contact)
        // -Wyświetl komunikat czy zaktualizowano

        int id = ReadInt("Podaj ID :");

        var c = new Contact
        {
            Id = id,
            FirstName = ReadRequired("Nowe imię: "),
            LastName = ReadRequired("Nowe nazwisko: "),
            Phone = ReadOptional("Nowy telefon: "),
            Email = ReadOptional("Nowy email: ")
        };
        bool updated = repo.Update(c);

        if (updated)
        {
            Console.WriteLine("Zaktualizowano kontakt.");
        }
        else
        {
            Console.WriteLine("Nie znaleziono kontaktu.");
        }
    }

    // =========================

    // DELETE

    // =========================

    static void Delete(ContactRepository repo)

    {

        // TODO (student):
        // -Pobierz ID (ReadInt)
        // -Wywoł aj repo.Delete(id)
        // -Wyś wietl komunikat czy usuni ęto

        int id = ReadInt("Podaj ID do usunięcia:");
        bool deleted = repo.Delete(id);

        if (deleted)
        {
            Console.WriteLine("Usunięto kontakt.");
        }
        else
        {
            Console.WriteLine("Nie usunięto kontakt.");
        }
    }

    // =========================

    // TRANSACTION (BULK INSERT)

    // =========================

    static void BulkInsertDemo(ContactRepository repo)

    {

        // TODO(student) – wersja podstawowa:
        // -Utwórz list ę kilku Contact (np. 3 rekordy)
        // -Wywołaj repo.BulkInsert(lista)
        // -Wyświetl ile dodano

        var lista = new List<Contact>
    {
        new Contact { FirstName = "Michał", LastName = "S" }, // Bez opcjonalnych danych
        new Contact { FirstName = "Łukasz", LastName = "W" }, // Bez opcjonalnych danych
    };

        int count = repo.BulkInsert(lista);
        Console.WriteLine($"Dodano {count} rekordów.");

    }

    // ==========================================================

    // HELPERS

    // ==========================================================

    static string ReadRequired(string label)

    {

        while (true)

        {
            Console.Write(label);

            string s = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(s)) return s.Trim();

            Console.WriteLine("Pole nie mo ż e by ć puste.");

        }

    }

    static string? ReadOptional(string label)

    {

        Console.Write(label);

        string s = Console.ReadLine() ?? "";

        s = s.Trim();

        return string.IsNullOrWhiteSpace(s) ? null : s;

    }

    static int ReadInt(string label)

    {

        while (true)

        {

            Console.Write(label);

            if (int.TryParse(Console.ReadLine(), out int id)) return id;

            Console.WriteLine("Podaj poprawn ą liczb ę ca ł kowit ą.");

        }

    }

}
