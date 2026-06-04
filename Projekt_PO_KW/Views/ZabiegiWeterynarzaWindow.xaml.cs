using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Projekt_PO_KW.Views
{
    public class ZabiegVM
    {
        public int IdZabieg { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public bool Przypisany { get; set; }
    }

    public partial class ZabiegiWeterynarzaWindow : Window
    {
        private readonly int _idWeterynarz;
        private List<ZabiegVM> _zabiegi = new();

        public ZabiegiWeterynarzaWindow(int idWeterynarz, string imieNazwisko)
        {
            InitializeComponent();
            _idWeterynarz = idWeterynarz;
            EtykietaWet.Text = $"Dr {imieNazwisko}";
            WczytajZabiegi();
        }

        private void WczytajZabiegi()
        {
            using var Conn = Database.GetConnection();
            Conn.Open();

            var commandAll = new SqlCommand("SELECT id_zabieg, nazwa FROM Zabieg", Conn);
            using var r1 = commandAll.ExecuteReader();
            while (r1.Read())
                _zabiegi.Add(new ZabiegVM
                {
                    IdZabieg = (int)r1["id_zabieg"],
                    Nazwa = r1["nazwa"].ToString()!
                });
            r1.Close();

            var coomandPrzyp = new SqlCommand("SELECT id_zabieg FROM Weterynarz_Zabieg WHERE id_weterynarz = @id", Conn);
            coomandPrzyp.Parameters.AddWithValue("@id", _idWeterynarz);
            using var r2 = coomandPrzyp.ExecuteReader();
            var przypisane = new HashSet<int>();
            while (r2.Read())
                przypisane.Add((int)r2["id_zabieg"]);

            foreach (var z in _zabiegi)
                z.Przypisany = przypisane.Contains(z.IdZabieg);

            ListaZabiegow.ItemsSource = _zabiegi;
        }

        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var Conn = Database.GetConnection();
                Conn.Open();

                var coomandDel = new SqlCommand("DELETE FROM Weterynarz_Zabieg WHERE id_weterynarz = @id", Conn);
                coomandDel.Parameters.AddWithValue("@id", _idWeterynarz);
                coomandDel.ExecuteNonQuery();

                foreach (var z in _zabiegi.Where(z => z.Przypisany))
                {
                    var coomandIns = new SqlCommand("INSERT INTO Weterynarz_Zabieg (id_weterynarz, id_zabieg) VALUES (@wet, @zab)", Conn);
                    coomandIns.Parameters.AddWithValue("@wet", _idWeterynarz);
                    coomandIns.Parameters.AddWithValue("@zab", z.IdZabieg);
                    coomandIns.ExecuteNonQuery();
                }

                System.Windows.MessageBox.Show("Zabiegi zostały zaktualizowane!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}