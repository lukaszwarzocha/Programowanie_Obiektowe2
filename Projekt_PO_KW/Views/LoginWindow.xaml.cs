using Projekt_PO_KW.Models;
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
using Projekt_PO_KW.Repositories;
using System.Diagnostics.Eventing.Reader;

namespace Projekt_PO_KW.Views
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Zaloguj_sie(object sender, RoutedEventArgs e)
        {
            var email = PoleEmail.Text;
            var haslo = PoleHaslo.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(haslo))
            {
                System.Windows.MessageBox.Show("Nieprawidłowy email lub hasło!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var rep = new UzytkownikRep();
                var uzytkownik = rep.GetUser(email, haslo);

                if (uzytkownik == null)
                {
                    System.Windows.MessageBox.Show("Nieprawidłowy login lub hasło!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (uzytkownik.Rola == "Administrator")
                    new AdminWindow().Show();
                else
                    Helpers.SessionHelper.ZalogowanyUzytkownik = uzytkownik;
                    new MainWindow().Show();

                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Nie udało ci się połączyć z bazą danych: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Zarejestruj_sie(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().Show();
            this.Close();
        }
    }
}
