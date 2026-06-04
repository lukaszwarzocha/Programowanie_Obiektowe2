using Microsoft.Data.SqlClient;
using Projekt_PO_KW.Models;
using System.ComponentModel.Design;

namespace Projekt_PO_KW.Repositories
{
    public class WeterynarzRep
    {
        public List<Weterynarz> GetAll()
        {
            var lista = new List<Weterynarz>();

            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("SELECT * FROM Weterynarz", Conn);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(MapujWeterynarza(reader));
            }

            return lista;
        }

        public List<Weterynarz> GetByZabieg(int idZabieg)
        {
            var lista = new List<Weterynarz>();

            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("SELECT w.* FROM Weterynarz w INNER JOIN Weterynarz_Zabieg wz ON w.id_weterynarz = wz.id_weterynarz WHERE wz.id_zabieg = @id", Conn);
            command.Parameters.AddWithValue("@id", idZabieg);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapujWeterynarza(reader));
            }

            return lista;
        }

        private Weterynarz MapujWeterynarza(SqlDataReader reader)
        {
            return new Weterynarz
            {
                IdWeterynarz = (int)reader["id_weterynarz"],
                Imie = reader["imie"].ToString()!,
                Nazwisko = reader["nazwisko"].ToString()!,
                Telefon = reader["telefon"].ToString()!,
                Specjalizacja = reader["specjalizacja"].ToString()!,
                Email = reader["email"].ToString()!
            };
        }

        public void Usun(int idWeterynarz)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var commandRez = new SqlCommand("DELETE FROM Rezerwacja WHERE id_weterynarz = @id", Conn);
            commandRez.Parameters.AddWithValue("@id", idWeterynarz);
            commandRez.ExecuteNonQuery();

            var command = new SqlCommand("DELETE FROM Weterynarz WHERE id_weterynarz = @id", Conn);
            command.Parameters.AddWithValue("@id", idWeterynarz);
            command.ExecuteNonQuery();
        }

        public void Dodaj(Weterynarz w)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand(
                "INSERT INTO Weterynarz (imie, nazwisko, telefon, specjalizacja, email, haslo) " +
                "VALUES (@imie, @nazwisko, @telefon, @specjalizacja, @email, @haslo)", Conn);
            command.Parameters.AddWithValue("@imie", w.Imie);
            command.Parameters.AddWithValue("@nazwisko", w.Nazwisko);
            command.Parameters.AddWithValue("@telefon", w.Telefon);
            command.Parameters.AddWithValue("@specjalizacja", w.Specjalizacja);
            command.Parameters.AddWithValue("@email", w.Email);
            command.Parameters.AddWithValue("@haslo", w.Haslo);
            command.ExecuteNonQuery();
        }
    }
}