using Microsoft.Data.SqlClient;
using Projekt_PO_KW.Models;

namespace Projekt_PO_KW.Repositories
{
    public class ZabiegRep
    {
        public List<Zabieg> GetAll()
        {
            var lista = new List<Zabieg>();

            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("SELECT * FROM Zabieg", Conn);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Zabieg
                {
                    IdZabieg = (int)reader["id_zabieg"],
                    Nazwa = reader["nazwa"].ToString()!,
                    Opis = reader["opis"].ToString()!,
                    Cena = (decimal)reader["cena"],
                    CzasTrwaniaMin = (int)reader["czas_trwania_min"]
                });
            }

            return lista;
        }

        public void Usun(int idZabieg)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var coomandRez = new SqlCommand("DELETE FROM Rezerwacja WHERE id_zabieg = @id", Conn);
            coomandRez.Parameters.AddWithValue("@id", idZabieg);
            coomandRez.ExecuteNonQuery();

            var coomandWZ = new SqlCommand("DELETE FROM Weterynarz_Zabieg WHERE id_zabieg = @id", Conn);
            coomandWZ.Parameters.AddWithValue("@id", idZabieg);
            coomandWZ.ExecuteNonQuery();

            var coomand = new SqlCommand("DELETE FROM Zabieg WHERE id_zabieg = @id", Conn);
            coomand.Parameters.AddWithValue("@id", idZabieg);
            coomand.ExecuteNonQuery();
        }

        public void Dodaj(Zabieg z)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var coomand = new SqlCommand("INSERT INTO Zabieg (nazwa, opis, cena, czas_trwania_min) VALUES (@nazwa, @opis, @cena, @czas)", Conn);
            coomand.Parameters.AddWithValue("@nazwa", z.Nazwa);
            coomand.Parameters.AddWithValue("@opis", (object?)z.Opis ?? DBNull.Value);
            coomand.Parameters.AddWithValue("@cena", z.Cena);
            coomand.Parameters.AddWithValue("@czas", z.CzasTrwaniaMin);
            coomand.ExecuteNonQuery();
        }
    }
}