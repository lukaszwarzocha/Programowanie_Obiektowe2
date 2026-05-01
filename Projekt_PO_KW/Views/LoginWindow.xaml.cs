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

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(haslo) ) {
                System.Windows.MessageBox.Show("Nieprawidłowy email lub hasło!");
                return;
            }

        }

        private void Zarejestruj_sie(object sender, RoutedEventArgs e)
        {

        }
    }
}
