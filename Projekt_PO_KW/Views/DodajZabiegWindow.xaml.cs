using System;
using System.Windows;

namespace Projekt_PO_KW.Views
{
    public partial class DodajZabiegWindow : Window
    {
        public DodajZabiegWindow() => InitializeComponent();

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            var nazwa = PoleNazwa.Text;
            var czasTxt = PoleCzas.Text;
            var cenaTxt = PoleCena.Text;

            if (string.IsNullOrEmpty(nazwa) || string.IsNullOrEmpty(czasTxt) || string.IsNullOrEmpty(cenaTxt))
            {
                System.Windows.MessageBox.Show("Musisz wypełnić wszystkie pola!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(czasTxt, out int czas) || czas <= 0)
            {
                System.Windows.MessageBox.Show("Musisz podać prawidłowy czas trwania (minuty)!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(cenaTxt, out decimal cena) || cena < 0)
            {
                System.Windows.MessageBox.Show("Musisz podać prawidłową cenę!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var rep = new Repositories.ZabiegRep();
                rep.Dodaj(new Models.Zabieg
                {
                    Nazwa = nazwa,
                    CzasTrwaniaMin = czas,
                    Cena = cena
                });

                System.Windows.MessageBox.Show("Zabieg został dodany pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}