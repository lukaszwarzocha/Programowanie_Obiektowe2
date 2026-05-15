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
    /// Logika interakcji dla klasy DodajPupilaWindow.xaml
    /// </summary>
    public partial class DodajPupilaWindow : Window
    {
        public DodajPupilaWindow()
        {
            InitializeComponent();
        }

        private void Dodaj_Pupila(object sender, RoutedEventArgs e)
        {
            var imie = PupilImie.Text;
            var gatunek = (PupilGatunek.SelectedItem as ComboBoxItem)?.Content?.ToString();
            var rasa = PupilRasa.Text;
            var wiektest = PupilWiek.Text;
            var plec = (PupilPlec.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (string.IsNullOrEmpty(imie) || string.IsNullOrEmpty(gatunek) || string.IsNullOrEmpty(rasa) || string.IsNullOrEmpty(wiektest) || string.IsNullOrEmpty(plec))
            {
                System.Windows.MessageBox.Show("Musisz wypełnić wszystkie pola!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(wiektest, out int wiek) || wiek < 0 || wiek > 30)
            {
                System.Windows.MessageBox.Show("Podaj prawidłowy wiek!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var pupil = new Models.Pupil
                {
                    Imie = imie,
                    Gatunek = gatunek,
                    Rasa = rasa,
                    Wiek = wiek,
                    Plec = plec!
                };

                var rep = new Repositories.PupilRep();
                rep.Dodaj(pupil, Helpers.SessionHelper.ZalogowanyUzytkownik!.IdUzytkownik);

                System.Windows.MessageBox.Show("Pupil został dodany pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas dodawania pupila: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
