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

namespace SMSProject
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = userName.Text.Trim();
            string haslo = password.Password.Trim();

            // Walidacja pustych pól
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(haslo))
            {
                MessageBox.Show(
                "Wprowadź login i hasło.",
                "Brak danych",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
                );

                userName.Clear();
                password.Clear();
                userName.Focus();
                return;
            }
            if (login == "admin" && haslo == "admin123")
            {
                MainWindow mainForm = new MainWindow();
                mainForm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(
                "Nieprawidłowy login lub hasło.",
                "Błąd logowania",
                MessageBoxButton.OK,
                MessageBoxImage.Error
                );

                // CZYSZCZENIE PÓL PO BŁĘDZIE
                // ustawienie kursora na login
            }
        }
    }
}
