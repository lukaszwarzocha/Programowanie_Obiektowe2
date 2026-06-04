using Microsoft.Data.SqlClient;
using Projekt_PO_KW.Models;
using Projekt_PO_KW.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Projekt_PO_KW.Views
{
    public partial class AdminWindow : Window
    {
        private List<RezerwacjaAdmin> _wszystkieRezerwacje = new();
        private bool _inicjalizacja = true;

        public AdminWindow()
        {
            InitializeComponent();
            EtykietaAdmin.Content = $"Witaj, {Helpers.SessionHelper.ZalogowanyUzytkownik?.Imie}!";
            WczytajWeterynarzy();

            var wetRep = new WeterynarzRep();
            var wets = wetRep.GetAll();
            wets.Insert(0, new Weterynarz { IdWeterynarz = 0, Imie = "Wszyscy", Nazwisko = "weterynarze" });
            FiltrWeterynarz.ItemsSource = wets;
            FiltrWeterynarz.SelectedIndex = 0;
            _inicjalizacja = false;
        }

        private void Zakladka_Click(object sender, RoutedEventArgs e)
        {
            PanelWeterynarze.Visibility = Visibility.Collapsed;
            PanelZabiegi.Visibility = Visibility.Collapsed;
            PanelRezerwacje.Visibility = Visibility.Collapsed;
            PanelUzytkownicy.Visibility = Visibility.Collapsed;

            BtnZakladkaWeterynarze.Tag = null;
            BtnZakladkaZabiegi.Tag = null;
            BtnZakladkaRezerwacje.Tag = null;
            BtnZakladkaUzytkownicy.Tag = null;

            var button = sender as System.Windows.Controls.Button;
            button!.Tag = "active";

            if (button == BtnZakladkaWeterynarze)
            {
                PanelWeterynarze.Visibility = Visibility.Visible;
                WczytajWeterynarzy();
            }
            else if (button == BtnZakladkaZabiegi)
            {
                PanelZabiegi.Visibility = Visibility.Visible;
                WczytajZabiegi();
            }
            else if (button == BtnZakladkaRezerwacje)
            {
                PanelRezerwacje.Visibility = Visibility.Visible;
                WczytajRezerwacje();
            }
            else if (button == BtnZakladkaUzytkownicy)
            {
                PanelUzytkownicy.Visibility = Visibility.Visible;
                WczytajUzytkownikow();
            }
        }

        private void Wyloguj_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SessionHelper.ZalogowanyUzytkownik = null;
            new LoginWindow().Show();
            this.Close();
        }

        private void WczytajWeterynarzy()
        {
            try
            {
                var rep = new WeterynarzRep();
                ListaWeterynarzy.ItemsSource = rep.GetAll();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DodajWeterynarza_Click(object sender, RoutedEventArgs e)
        {
            new DodajWeterynarzaWindow().ShowDialog();
            WczytajWeterynarzy();
        }

        private void UsunWeterynarza_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var id = (int)button!.Tag;

            var potwierdzenie = System.Windows.MessageBox.Show("Usunięcie weterynarza usunie też jego dostępność i rezerwacje. Kontynuować?", 
                "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (potwierdzenie == MessageBoxResult.No) return;

            try
            {
                var rep = new WeterynarzRep();
                rep.Usun(id);
                WczytajWeterynarzy();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd usuwania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WczytajZabiegi()
        {
            try
            {
                var rep = new ZabiegRep();
                ListaZabiegow.ItemsSource = rep.GetAll();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DodajZabieg_Click(object sender, RoutedEventArgs e)
        {
            new DodajZabiegWindow().ShowDialog();
            WczytajZabiegi();
        }

        private void UsunZabieg_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var id = (int)button!.Tag;

            var potwierdzenie = System.Windows.MessageBox.Show("Usunięcie zabiegu usunie też powiązane rezerwacje. Kontynuować?", 
                "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (potwierdzenie == MessageBoxResult.No) return;

            try
            {
                var rep = new ZabiegRep();
                rep.Usun(id);
                WczytajZabiegi();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd usuwania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WczytajRezerwacje()
        {
            try
            {
                var rep = new RezerwacjaRep();
                _wszystkieRezerwacje = rep.GetAllAdmin();
                ZastosujFiltry();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtry_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (_inicjalizacja) return; 
            ZastosujFiltry();
        }

        private void ZastosujFiltry()
        {
            if (_inicjalizacja) return;
            if (_wszystkieRezerwacje == null) return;
            if (ListaRezerwacji == null) return;

            var wynik = _wszystkieRezerwacje.AsEnumerable();

            if (FiltrData.SelectedDate.HasValue) wynik = wynik.Where(r => r.DataRezerwacji.Date == FiltrData.SelectedDate.Value.Date);

            if (FiltrWeterynarz.SelectedItem is Weterynarz wet && wet.IdWeterynarz != 0) wynik = wynik.Where(r => r.IdWeterynarz == wet.IdWeterynarz);

            if (FiltrStatus.SelectedItem is ComboBoxItem statusItem && statusItem.Content?.ToString() != "Wszystkie statusy")
                wynik = wynik.Where(r => r.Status == statusItem.Content?.ToString());

            ListaRezerwacji.ItemsSource = wynik.ToList();
        }

        private void WyczyscFiltry_Click(object sender, RoutedEventArgs e)
        {
            FiltrData.SelectedDate = null;
            if (FiltrWeterynarz.Items.Count > 0) FiltrWeterynarz.SelectedIndex = 0;
            FiltrStatus.SelectedIndex = 0;
            ZastosujFiltry();
        }

        private void StatusRezerwacji_Changed(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as System.Windows.Controls.ComboBox;
            if (combo?.Tag == null) return;

            var idRezerwacja = (int)combo.Tag;
            var nowyStatus = (combo.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (nowyStatus == null) return;

            try
            {
                var rep = new RezerwacjaRep();
                rep.ZmienStatus(idRezerwacja, nowyStatus);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd zmiany statusu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WczytajUzytkownikow()
        {
            try
            {
                var rep = new UzytkownikRep();
                ListaUzytkownikow.ItemsSource = rep.GetAllUzytkownicy();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UsunUzytkownika_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var id = (int)button!.Tag;

            var potwierdzenie = System.Windows.MessageBox.Show("Usunięcie użytkownika usunie też jego pupile i rezerwacje. Kontynuować?",
                "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (potwierdzenie == MessageBoxResult.No) return;

            try
            {
                var rep = new UzytkownikRep();
                rep.UsunUzytkownika(id);
                WczytajUzytkownikow();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd usuwania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}