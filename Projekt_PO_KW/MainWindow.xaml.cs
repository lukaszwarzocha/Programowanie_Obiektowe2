using Projekt_PO_KW.Repositories;
using Projekt_PO_KW.Views;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt_PO_KW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            EtykietaUzytkownik.Content = $"Witaj, {Helpers.SessionHelper.ZalogowanyUzytkownik?.Imie}!";
            WczytajDane();
        }

        private void WczytajDane()
        {
            var uzytkownik = Helpers.SessionHelper.ZalogowanyUzytkownik;
            if (uzytkownik == null) return;

            TwojeImie.Text = uzytkownik.Imie;
            TwojeNazwisko.Text = uzytkownik.Nazwisko;
            TwojEmail.Text = uzytkownik.Email;
            TwojTelefon.Text = uzytkownik.Telefon;
            TwojAdres.Text = uzytkownik.Adres;
        }

        private void Zmiana_danych(object sender, RoutedEventArgs e)
        {
            var uzytkownik = Helpers.SessionHelper.ZalogowanyUzytkownik;
            if (uzytkownik == null) return;

            var imie = TwojeImie.Text;
            var nazwisko = TwojeNazwisko.Text;
            var email = TwojEmail.Text;
            var telefon = TwojTelefon.Text;
            var adres = TwojAdres.Text;

            if (imie == uzytkownik.Imie && nazwisko == uzytkownik.Nazwisko && email == uzytkownik.Email && telefon == uzytkownik.Telefon && adres == uzytkownik.Adres)
            {
                System.Windows.MessageBox.Show("Wprowadź dane do zmiany!.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (email != uzytkownik.Email)
            {
                var rep = new Repositories.UzytkownikRep();
                if (rep.CzyEmailZajety(email))
                {
                    System.Windows.MessageBox.Show("Podany email jest już zajęty!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            var potwierdz = System.Windows.MessageBox.Show("Czy na pewno chcesz zmienić dane?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            try
            {
                uzytkownik.Imie = imie;
                uzytkownik.Nazwisko = nazwisko;
                uzytkownik.Email = email;
                uzytkownik.Telefon = telefon;
                uzytkownik.Adres = adres;

                var rep = new Repositories.UzytkownikRep();
                rep.Zmiana_danych(uzytkownik);

                System.Windows.MessageBox.Show("Dane zostały pomyślnie zaktualizowane!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                WczytajDane();
                EtykietaUzytkownik.Content = $"Witaj, {Helpers.SessionHelper.ZalogowanyUzytkownik?.Imie}!";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd aktualizacji: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Zmiana_hasla(object sender, RoutedEventArgs e)
        {
            var uzytkownik = Helpers.SessionHelper.ZalogowanyUzytkownik;
            if (uzytkownik == null) return;

            var aktualne = AktualneHaslo.Password;
            var nowe = NoweHaslo.Password;
            var potwierdz_haslo = PowtorzHaslo.Password;

            if (string.IsNullOrEmpty(aktualne) || string.IsNullOrEmpty(nowe) || string.IsNullOrEmpty(potwierdz_haslo))
            {
                System.Windows.MessageBox.Show("Pierw musisz wypełnić wszystkie pola dotyczące hasła!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (aktualne != uzytkownik.Haslo)
            {
                System.Windows.MessageBox.Show("Obecne hasło jest nieprawidłowe!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (nowe != potwierdz_haslo)
            {
                System.Windows.MessageBox.Show("Nowe hasło musi być takie same jak w polu potwierdzającym!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (nowe.Length < 6)
            {
                System.Windows.MessageBox.Show("Hasło musi mieć minimum 6 znaków!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var potwierdz = System.Windows.MessageBox.Show("Czy na pewno chcesz zmienić hasło?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (potwierdz == MessageBoxResult.No) return;

            try
            {
                uzytkownik.Haslo = nowe;
                var rep = new Repositories.UzytkownikRep();
                rep.Zmiana_hasla(uzytkownik);

                AktualneHaslo.Clear(); NoweHaslo.Clear(); PowtorzHaslo.Clear();

                System.Windows.MessageBox.Show("Twoje hasło zostało zmienione!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas zmiany hasła: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Wyloguj(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }

        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            PanelMojeKonto.Visibility = Visibility.Collapsed;
            PanelMojePupile.Visibility = Visibility.Collapsed;
            PanelMojeWizyty.Visibility = Visibility.Collapsed;

            BtnMojeKonto.Tag = null;
            BtnMojePupile.Tag = null;
            BtnMojeWizyty.Tag = null;

            var przycisk = sender as System.Windows.Controls.Button;
            przycisk!.Tag = "active";

            if (przycisk == BtnMojeKonto)
                PanelMojeKonto.Visibility = Visibility.Visible;
            else if (przycisk == BtnMojePupile)
            {
                PanelMojePupile.Visibility = Visibility.Visible;
                WczytajPupile();
            }
            else if (przycisk == BtnMojeWizyty)
            {
                PanelMojeWizyty.Visibility = Visibility.Visible;
                WczytajWizyty();
            }
        }

        private void WczytajPupile()
        {
            try
            {
                var rep = new Repositories.PupilRep();
                var pupile = rep.GetByUzytkownik(Helpers.SessionHelper.ZalogowanyUzytkownik!.IdUzytkownik);
                ListaPupili.ItemsSource = pupile;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas wczytywania twoich pupili: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UsunPupila_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var idPupil = (int)button!.Tag;

            var potwierdzenie = System.Windows.MessageBox.Show("Czy na pewno chcesz usunąć tego pupila?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (potwierdzenie == MessageBoxResult.No) return;

            try
            {
                var rep = new Repositories.PupilRep();
                rep.Usun(idPupil);
                WczytajPupile();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas usuwania pupila: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EdytujPupila_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var idPupil = (int)button!.Tag;

            var input = Helpers.DialogHelper.Show("Edytuj wiek pupila", "Podaj nowy wiek:");
            if (input == null) return;

            if (!int.TryParse(input, out int nowyWiek) || nowyWiek < 0 || nowyWiek > 30)
            {
                System.Windows.MessageBox.Show("Podaj prawidłowy wiek!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var rep = new Repositories.PupilRep();
                rep.ZmienWiek(idPupil, nowyWiek);
                WczytajPupile();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas zmiany wieku pupila: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WczytajWizyty()
        {
            var rep = new Repositories.RezerwacjaRep();
            var wizyty = rep.GetByUzytkownik(Helpers.SessionHelper.ZalogowanyUzytkownik!.IdUzytkownik);
            ListaWizyt.ItemsSource = wizyty;
        }

        private void DodajPupila_Click(object sender, RoutedEventArgs e)
        {
            new Views.DodajPupilaWindow().ShowDialog();
            if (PanelMojePupile.Visibility == Visibility.Visible) WczytajPupile();
        }

        private void ZarezerwujWizyte_Click(object sender, RoutedEventArgs e)
        {
            new Views.RezerwacjaWindow().ShowDialog();
        }

        private void AnulujWizyte_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var idRezerwacja = (int)button!.Tag;

            var potwierdzenie = System.Windows.MessageBox.Show("Czy na pewno chcesz anulować tę wizytę?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (potwierdzenie == MessageBoxResult.No) return;

            try
            {
                var rep = new Repositories.RezerwacjaRep();
                rep.Anuluj(idRezerwacja);
                WczytajWizyty();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas anulowania wizyty: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
