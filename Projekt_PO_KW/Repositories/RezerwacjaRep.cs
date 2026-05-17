using Microsoft.Data.SqlClient;
using Projekt_PO_KW.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_PO_KW.Repositories
{
    public class RezerwacjaRep
    {
        public List<Rezerwacja> GetByWeterynarz(int idWeterynarz)
        {
            var lista = new List<Rezerwacja>();

            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("SELECT * FROM Rezerwacja WHERE id_weterynarz = @id AND status != 'Anulowany'", Conn);
            command.Parameters.AddWithValue("@id", idWeterynarz);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Rezerwacja
                {
                    IdRezerwacja = (int)reader["id_rezerwacja"],
                    IdWeterynarz = (int)reader["id_weterynarz"],
                    IdPupil = (int)reader["id_pupil"],
                    IdZabieg = (int)reader["id_zabieg"],
                    IdUzytkownik = (int)reader["id_uzytkownik"],
                    DataRezerwacji = (DateTime)reader["data_rezerwacji"],
                    Godzina_Start = (TimeSpan)reader["godzina_start"],
                    Godzina_Koniec = (TimeSpan)reader["godzina_koniec"],
                    Status = reader["status"].ToString()!
                });
            }

            return lista;
        }

        public List<Rezerwacja> GetByUzytkownik(int idUzytkownik)
        {
            var lista = new List<Rezerwacja>();

            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("SELECT r.*, z.nazwa AS nazwa_zabiegu, w.imie + ' ' + w.nazwisko AS imie_nazwisko_weterynarza, p.imie AS imie_pupila " +
            "FROM Rezerwacja r INNER JOIN Zabieg z ON r.id_zabieg = z.id_zabieg INNER JOIN Weterynarz w ON r.id_weterynarz = w.id_weterynarz INNER JOIN Pupil p ON r.id_pupil = p.id_pupil " +
            "WHERE r.id_uzytkownik = @id ORDER BY r.data_rezerwacji DESC", Conn);
            command.Parameters.AddWithValue("@id", idUzytkownik);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Rezerwacja
                {
                    IdRezerwacja = (int)reader["id_rezerwacja"],
                    IdWeterynarz = (int)reader["id_weterynarz"],
                    IdPupil = (int)reader["id_pupil"],
                    IdZabieg = (int)reader["id_zabieg"],
                    IdUzytkownik = (int)reader["id_uzytkownik"],
                    DataRezerwacji = (DateTime)reader["data_rezerwacji"],
                    Godzina_Start = (TimeSpan)reader["godzina_start"],
                    Godzina_Koniec = (TimeSpan)reader["godzina_koniec"],
                    Status = reader["status"].ToString()!,
                    NazwaZabiegu = reader["nazwa_zabiegu"].ToString()!,
                    ImieNazwiskoWeterynarza = reader["imie_nazwisko_weterynarza"].ToString()!,
                    ImiePupila = reader["imie_pupila"].ToString()!
                });
            }

            return lista;
        }

        public void Dodaj(Rezerwacja r)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand(
                "INSERT INTO Rezerwacja (id_weterynarz, id_pupil, id_zabieg, id_uzytkownik, data_rezerwacji, godzina_start, godzina_koniec, status) " +
                "VALUES (@idWet, @idPupil, @idZabieg, @idUzyt, @data, @godzStart, @godzKoniec, @status)", Conn);

            command.Parameters.AddWithValue("@idWet", r.IdWeterynarz);
            command.Parameters.AddWithValue("@idPupil", r.IdPupil);
            command.Parameters.AddWithValue("@idZabieg", r.IdZabieg);
            command.Parameters.AddWithValue("@idUzyt", r.IdUzytkownik);
            command.Parameters.AddWithValue("@data", r.DataRezerwacji);
            command.Parameters.AddWithValue("@godzStart", r.Godzina_Start);
            command.Parameters.AddWithValue("@godzKoniec", r.Godzina_Koniec);
            command.Parameters.AddWithValue("@status", r.Status);

            command.ExecuteNonQuery();
        }

        public void Anuluj(int idRezerwacja)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand("UPDATE Rezerwacja SET status = 'Anulowany' WHERE id_rezerwacja = @id", Conn);
            command.Parameters.AddWithValue("@id", idRezerwacja);
            command.ExecuteNonQuery();
        }
    }
}
