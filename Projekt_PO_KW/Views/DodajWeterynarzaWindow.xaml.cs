using System;
using System.Windows;

namespace Projekt_PO_KW.Views
{
    public partial class DodajWeterynarzaWindow : Window
    {
        public DodajWeterynarzaWindow() => InitializeComponent();

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            var imie = PoleImie.Text;
            var nazwisko = PoleNazwisko.Text;
            var specjalizacja = PoleSpecjalizacja.Text;
            var email = PoleEmail.Text;
            var telefon = PoleTelefon.Text;
            var haslo = PoleHaslo.Password;

            if (string.IsNullOrEmpty(imie) || string.IsNullOrEmpty(nazwisko) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(haslo))
            {
                System.Windows.MessageBox.Show("Musisz wypełnić wymagane pola (imię, nazwisko, email, hasło)!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var rep = new Repositories.WeterynarzRep();
                rep.Dodaj(new Models.Weterynarz
                {
                    Imie = imie,
                    Nazwisko = nazwisko,
                    Specjalizacja = specjalizacja,
                    Email = email,
                    Telefon = telefon,
                    Haslo = haslo
                });

                System.Windows.MessageBox.Show("Weterynarz został dodany pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}