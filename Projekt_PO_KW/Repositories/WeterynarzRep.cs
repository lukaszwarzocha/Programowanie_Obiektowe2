using Microsoft.Data.SqlClient;
using Projekt_PO_KW.Models;

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
    }
}