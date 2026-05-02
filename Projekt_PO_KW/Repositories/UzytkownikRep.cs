using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Animation;
using Microsoft.Data.SqlClient;
using Projekt_PO_KW.Models;

namespace Projekt_PO_KW.Repositories
{
    public class UzytkownikRep
    {

        public Uzytkownik? GetUser(string email, string password) 
        { 
        using var Conn = Database.GetConnection();
        Conn.Open();

        var command = new SqlCommand("SELECT * FROM Uzytkownik WHERE email = @email AND haslo = @haslo", Conn);
        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@haslo", password);

        using var reader = command.ExecuteReader();
        if (reader.Read())
            {
                return new Uzytkownik
                {
                    IdUzytkownik = (int)reader["id_uzytkownik"],
                    Imie = reader["imie"].ToString()!,
                    Nazwisko = reader["nazwisko"].ToString()!,
                    Email = reader["email"].ToString()!,
                    Telefon = reader["telefon"].ToString()!,
                    Rola = reader["rola"].ToString()!
                };
            }

            return null;
        }

        public void AddUser(Uzytkownik user) 
        { 
        using var Conn = Database.GetConnection();
        Conn.Open();

        var cmd = new SqlCommand("INSERT INTO Uzytkownik (imie, nazwisko, adres, email, haslo, telefon, rola) " +
        "VALUES (@imie, @nazwisko, @adres, @email, @haslo, @telefon, @rola)", Conn);
        cmd.Parameters.AddWithValue("@imie", user.Imie);
        cmd.Parameters.AddWithValue("@nazwisko", user.Nazwisko);
        cmd.Parameters.AddWithValue("@adres", user.Adres);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@haslo", user.Haslo);
        cmd.Parameters.AddWithValue("@telefon", user.Telefon);
        cmd.Parameters.AddWithValue("@rola", user.Rola);

        cmd.ExecuteNonQuery();
        }
    }
}
