using Microsoft.Data.SqlClient;
using Projekt_PO_KW.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_PO_KW.Repositories
{
    public class PupilRep
    {
        public void Dodaj(Models.Pupil pupil, int idUzytkownik)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("INSERT INTO Pupil (imie, gatunek, wiek, rasa) VALUES (@imie, @gatunek, @wiek, @rasa, @plec) SELECT SCOPE_IDENTITY();", Conn);
            command.Parameters.AddWithValue("@imie", pupil.Imie);
            command.Parameters.AddWithValue("@gatunek", pupil.Gatunek);
            command.Parameters.AddWithValue("@wiek", pupil.Wiek);
            command.Parameters.AddWithValue("@rasa", pupil.Rasa);
            command.Parameters.AddWithValue("@plec", pupil.Plec);

            int idPupil = Convert.ToInt32(command.ExecuteScalar());
            var command2 = new SqlCommand("INSERT INTO Uzytkownik_Pupil (id_uzytkownik, id_pupil) VALUES (@idUzytkownik, @idPupil)", Conn);
            command2.Parameters.AddWithValue("@idUzytkownik", idUzytkownik);
            command2.Parameters.AddWithValue("@idPupil", idPupil);
            command2.ExecuteNonQuery();
        }
        public List<Pupil> GetByUzytkownik(int idUzytkownik)
        {
            var lista = new List<Pupil>();

            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("SELECT p.* FROM Pupil p INNER JOIN Uzytkownik_Pupil up ON p.id_pupil = up.id_pupil WHERE up.id_uzytkownik = @id;", Conn);
            command.Parameters.AddWithValue("@id", idUzytkownik);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Pupil
                {
                    IdPupil = (int)reader["id_pupil"],
                    Imie = reader["imie"].ToString()!,
                    Gatunek = reader["gatunek"].ToString()!,
                    Wiek = (int)reader["wiek"]!,
                    Rasa = reader["rasa"].ToString()!,
                    Plec = reader["plec"].ToString()!
                });
            }

            return lista;
        }

        public void Usun(int idPupil)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("DELETE FROM PUPIL WHERE id_pupil = @id", Conn);
            command.Parameters.AddWithValue("@id", idPupil);
            command.ExecuteNonQuery();
        }

        public void ZmienWiek(int idPupil, int nowyWiek)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("UPDATE Pupil SET wiek = @wiek WHERE id_pupil = @id", Conn);
            command.Parameters.AddWithValue("@wiek", nowyWiek);
            command.Parameters.AddWithValue("@id", idPupil);
            command.ExecuteNonQuery();
        }
    }
}
