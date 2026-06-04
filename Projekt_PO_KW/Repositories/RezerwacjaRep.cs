using Microsoft.Data.SqlClient;
using Projekt_PO_KW.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;

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

        public List<RezerwacjaAdmin> GetAllAdmin()
        {
            var lista = new List<RezerwacjaAdmin>();

            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand(
                "SELECT r.id_rezerwacja, r.id_weterynarz, r.data_rezerwacji, " +
                "r.godzina_start, r.godzina_koniec, r.status, " +
                "w.imie + ' ' + w.nazwisko AS imie_nazwisko_weterynarza, " +
                "p.imie AS imie_pupila, " +
                "z.nazwa AS nazwa_zabiegu, " +
                "u.imie + ' ' + u.nazwisko AS imie_nazwisko_uzytkownika " +
                "FROM Rezerwacja r " +
                "INNER JOIN Weterynarz w ON r.id_weterynarz = w.id_weterynarz " +
                "INNER JOIN Pupil p ON r.id_pupil = p.id_pupil " +
                "INNER JOIN Zabieg z ON r.id_zabieg = z.id_zabieg " +
                "INNER JOIN Uzytkownik u ON r.id_uzytkownik = u.id_uzytkownik " +
                "ORDER BY r.data_rezerwacji DESC, r.godzina_start", Conn);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new RezerwacjaAdmin
                {
                    IdRezerwacja = (int)reader["id_rezerwacja"],
                    IdWeterynarz = (int)reader["id_weterynarz"],
                    DataRezerwacji = (DateTime)reader["data_rezerwacji"],
                    GodzStart = (TimeSpan)reader["godzina_start"],
                    GodzKoniec = (TimeSpan)reader["godzina_koniec"],
                    Status = reader["status"].ToString()!,
                    ImieNazwiskoWeterynarza = reader["imie_nazwisko_weterynarza"].ToString()!,
                    ImiePupila = reader["imie_pupila"].ToString()!,
                    NazwaZabiegu = reader["nazwa_zabiegu"].ToString()!,
                    ImieNazwiskoUzytkownika = reader["imie_nazwisko_uzytkownika"].ToString()!
                });
            }

            return lista;
        }

        public void ZmienStatus(int idRezerwacja, string nowyStatus)
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var command = new SqlCommand(
                "UPDATE Rezerwacja SET status = @status WHERE id_rezerwacja = @id", Conn);
            command.Parameters.AddWithValue("@status", nowyStatus);
            command.Parameters.AddWithValue("@id", idRezerwacja);
            command.ExecuteNonQuery();
        }
    }
}
