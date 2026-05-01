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

namespace WpfAppLab2.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        string login = LoginBox.Text.Trim();
        string haslo = password.Password.Trim();

        // Walidacja pustych pól i
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(haslo)) {
            MessageBox.Show("Wprowadź login i hasło.", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);

            // Czyszczenie pól
            userName.Clear();
            password.Clear();
            userName.Focus();

            return;
        }
        // Sprawdzenie danych logowania
        if (login == "admin" && haslo == "admin123") {
            MainForm mainForm = new MainForm();
            mainForm.Show(); this.Close();
        }

        else {
            MessageBox.Show("Nieprawidłowy login lub hasło.", "Błąd logowania", MessageBoxButton.OK, MessageBoxImage.Error);

            // CZYSZCZENIE PÓL PO BŁĘDZIE // ustawienie kursora na login } }
        }
    }
