using Projekt_PO_KW.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekt_PO_KW.Views
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void WrocDoLogowania(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }

        private void Zarejestruj(object sender, RoutedEventArgs e)
        {
            var imie = PoleImie.Text;
            var nazwisko = PoleNazwisko.Text;
            var email = PoleEmail.Text;
            var telefon = PoleTelefon.Text;
            var adres = PoleAdres.Text;
            var haslo1 = PoleHaslo.Password;
            var haslo2 = PolePotwierdzHaslo.Password;

            if (string.IsNullOrEmpty(imie) || string.IsNullOrEmpty(nazwisko) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(telefon) || string.IsNullOrEmpty(haslo1) || string.IsNullOrEmpty(haslo2))
            {
                System.Windows.MessageBox.Show("Uzupełnij wszystkie wymagane pola!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                System.Windows.MessageBox.Show("Nieprawidłowy format email!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var rep1 = new UzytkownikRep();
            if (rep1.CzyEmailZajety(email))
            {
                System.Windows.MessageBox.Show("Podany przez ciebie email jest już zajęty!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (telefon.Length != 9)
            {
                System.Windows.MessageBox.Show("Niewłaściwy format numeru telefonu!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (haslo1 != haslo2)
            {
                System.Windows.MessageBox.Show("Hasła muszą być takie same!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (haslo1.Length < 6)
            {
                System.Windows.MessageBox.Show("Hasło musi mieć minimum 6 znaków!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var rep2 = new UzytkownikRep();
                var uzytkownik = new Models.Uzytkownik
                {
                    Imie = imie,
                    Nazwisko = nazwisko,
                    Email = email,
                    Telefon = telefon,
                    Adres = adres,
                    Haslo = haslo1,
                    Rola = "Uzytkownik"
                };

                rep2.AddUser(uzytkownik);
                System.Windows.MessageBox.Show("Konto zostało utworzone!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                new LoginWindow().Show();
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Nie udało ci się połączyć z bazą danych: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

