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
                    Rola = reader["rola"].ToString()!,
                    Saldo = (int)reader["Saldo"]!
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

        public void DoladujSaldo(int idUzytkownik, int kwota)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("UPDATE Uzytkownik SET Saldo = Saldo + @kwota WHERE id_uzytkownik = @id", Conn);
            command.Parameters.AddWithValue("@kwota", kwota);
            command.Parameters.AddWithValue("@id", idUzytkownik);
            command.ExecuteNonQuery();
        }

        public void PobierzSaldo(int idUzytkownik, int kwota)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("UPDATE Uzytkownik SET Saldo = Saldo - @kwota WHERE id_uzytkownik = @id", Conn);
            command.Parameters.AddWithValue("@kwota", kwota);
            command.Parameters.AddWithValue("@id", idUzytkownik);
            command.ExecuteNonQuery();
        }

        public List<Uzytkownik> GetAllUzytkownicy()
        {
            var lista = new List<Uzytkownik>();

            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand(
                "SELECT * FROM Uzytkownik WHERE rola = 'Uzytkownik' ORDER BY nazwisko", Conn);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Uzytkownik
                {
                    IdUzytkownik = (int)reader["id_uzytkownik"],
                    Imie = reader["imie"].ToString()!,
                    Nazwisko = reader["nazwisko"].ToString()!,
                    Email = reader["email"].ToString()!,
                    Telefon = reader["telefon"].ToString()!,
                    Saldo = (int)reader["Saldo"]
                });
            }

            return lista;
        }

        public void UsunUzytkownika(int idUzytkownik)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var coomandRez = new SqlCommand("DELETE FROM Rezerwacja WHERE id_uzytkownik = @id", Conn);
            coomandRez.Parameters.AddWithValue("@id", idUzytkownik);
            coomandRez.ExecuteNonQuery();

            var coomandPupile = new SqlCommand("SELECT id_pupil FROM Uzytkownik_Pupil WHERE id_uzytkownik = @id", Conn);
            coomandPupile.Parameters.AddWithValue("@id", idUzytkownik);
            var idPupili = new List<int>();
            using (var reader = coomandPupile.ExecuteReader())
                while (reader.Read()) idPupili.Add((int)reader["id_pupil"]);

            var coomandUP = new SqlCommand("DELETE FROM Uzytkownik_Pupil WHERE id_uzytkownik = @id", Conn);
            coomandUP.Parameters.AddWithValue("@id", idUzytkownik);
            coomandUP.ExecuteNonQuery();

            foreach (var idPupil in idPupili)
            {
                var coomandCheck = new SqlCommand("SELECT COUNT(*) FROM Uzytkownik_Pupil WHERE id_pupil = @id", Conn);
                coomandCheck.Parameters.AddWithValue("@id", idPupil);
                int count = (int)coomandCheck.ExecuteScalar();
                if (count == 0)
                {
                    var coomanddDelPupil = new SqlCommand("DELETE FROM Pupil WHERE id_pupil = @id", Conn);
                    coomanddDelPupil.Parameters.AddWithValue("@id", idPupil);
                    coomanddDelPupil.ExecuteNonQuery();
                }
            }

            var coomandUser = new SqlCommand("DELETE FROM Uzytkownik WHERE id_uzytkownik = @id", Conn);
            coomandUser.Parameters.AddWithValue("@id", idUzytkownik);
            coomandUser.ExecuteNonQuery();
        }
    }
}

