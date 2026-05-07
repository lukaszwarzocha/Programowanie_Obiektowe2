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
    }
}