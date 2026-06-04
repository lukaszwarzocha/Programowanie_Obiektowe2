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

                int idWet = rep.GetIdByEmail(email);

                var dostRep = new Repositories.DostepnoscRep();
                ZapiszDostepnosc(dostRep, idWet, 1, PonOd.Text, PonDo.Text);
                ZapiszDostepnosc(dostRep, idWet, 2, WtOd.Text, WtDo.Text);
                ZapiszDostepnosc(dostRep, idWet, 3, SrOd.Text, SrDo.Text);
                ZapiszDostepnosc(dostRep, idWet, 4, CzwOd.Text, CzwDo.Text);
                ZapiszDostepnosc(dostRep, idWet, 5, PtOd.Text, PtDo.Text);

                System.Windows.MessageBox.Show("Weterynarz został dodany pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ZapiszDostepnosc(Repositories.DostepnoscRep rep, int idWet, int dzien, string od, string doo)
        {
            if (string.IsNullOrWhiteSpace(od) || string.IsNullOrWhiteSpace(doo)) return;

            if (!TimeSpan.TryParse(od, out var gOd) || !TimeSpan.TryParse(doo, out var gDo) || gOd >= gDo) return;

            rep.Dodaj(new Models.Dostepnosc
            {
                IdWeterynarz = idWet,
                DzienTygodnia = dzien,
                GodzStart = gOd,
                GodzKoniec = gDo
            });
        }
    }
}
