using Microsoft.Data.SqlClient;
using Projekt_PO_KW.Models;

namespace Projekt_PO_KW.Repositories
{
    public class DostepnoscRep
    {
        public List<Dostepnosc> GetByWeterynarz(int idWeterynarz)
        {
            var lista = new List<Dostepnosc>();

            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("SELECT * FROM Dostepnosc WHERE id_weterynarz = @id", Conn);
            command.Parameters.AddWithValue("@id", idWeterynarz);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Dostepnosc
                {
                    IdGodziny = (int)reader["id_godziny"],
                    IdWeterynarz = (int)reader["id_weterynarz"],
                    DzienTygodnia = (int)reader["dzien_tygodnia"],
                    GodzStart = (TimeSpan)reader["godzina_start"],
                    GodzKoniec = (TimeSpan)reader["godzina_koniec"]
                });
            }

            return lista;
        }

        public void Dodaj(Models.Dostepnosc d)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("INSERT INTO Dostepnosc (id_weterynarz, dzien_tygodnia, godzina_start, godzina_koniec) VALUES (@id, @dzien, @od, @do)", Conn);
            command.Parameters.AddWithValue("@id", d.IdWeterynarz);
            command.Parameters.AddWithValue("@dzien", d.DzienTygodnia);
            command.Parameters.AddWithValue("@od", d.GodzStart);
            command.Parameters.AddWithValue("@do", d.GodzKoniec);
            command.ExecuteNonQuery();
        }
    }
}