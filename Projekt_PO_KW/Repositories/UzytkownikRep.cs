using System;
using System.Collections.Generic;
using System.Text;
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
                    Adres = reader["adres"].ToString()!,
                    Haslo = reader["haslo"].ToString()!,
                    Rola = reader["rola"].ToString()!
                };
            }

            return null;
        }

        public void AddUser(Uzytkownik user)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("INSERT INTO Uzytkownik (imie, nazwisko, adres, email, haslo, telefon, rola) " +
            "VALUES (@imie, @nazwisko, @adres, @email, @haslo, @telefon, @rola)", Conn);
            command.Parameters.AddWithValue("@imie", user.Imie);
            command.Parameters.AddWithValue("@nazwisko", user.Nazwisko);
            command.Parameters.AddWithValue("@adres", string.IsNullOrEmpty(user.Adres) ? DBNull.Value : user.Adres);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@haslo", user.Haslo);
            command.Parameters.AddWithValue("@telefon", user.Telefon);
            command.Parameters.AddWithValue("@rola", user.Rola);

            command.ExecuteNonQuery();
        }

        public bool CzyEmailZajety(string email)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("SELECT COUNT(*) FROM Uzytkownik WHERE email = @email", Conn);
            command.Parameters.AddWithValue("@email", email);
            return (int)command.ExecuteScalar() > 0;
        }

        public void Zmiana_danych(Uzytkownik user)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("UPDATE Uzytkownik SET imie = @imie, nazwisko = @nazwisko, email = @email, telefon = @telefon, adres = @adres Where id_uzytkownik = @id", Conn);
            command.Parameters.AddWithValue("@imie", user.Imie);
            command.Parameters.AddWithValue("@nazwisko", user.Nazwisko);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@telefon", user.Telefon);
            command.Parameters.AddWithValue("@adres", string.IsNullOrEmpty(user.Adres) ? DBNull.Value : user.Adres);
            command.Parameters.AddWithValue("@id", user.IdUzytkownik);

            command.ExecuteNonQuery();
        }

        public void Zmiana_hasla(Uzytkownik user)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("Update Uzytkownik SET haslo = @haslo WHERE id_uzytkownik = @id", Conn);
            command.Parameters.AddWithValue("@haslo", user.Haslo);
            command.Parameters.AddWithValue("@id", user.IdUzytkownik);

            command.ExecuteNonQuery();
        }
    }
}

