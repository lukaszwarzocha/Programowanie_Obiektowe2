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
    }
}