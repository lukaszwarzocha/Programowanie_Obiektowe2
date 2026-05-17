using Projekt_PO_KW.Repositories;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Projekt_PO_KW.Views
{
    public partial class RezerwacjaWindow : Window
    {
        private int _aktualnyKrok = 1;
        private Models.Pupil? _wybranyPupil;
        private Models.Zabieg? _wybranyZabieg;
        private Models.Weterynarz? _wybranyWeterynarz;
        private DateTime _wybranaData;
        private TimeSpan _wybranaGodzina;
        private System.Windows.Controls.Button? _wybranySlot;

        public RezerwacjaWindow()
        {
            InitializeComponent();
            WczytajDane();
        }

        private void WczytajDane()
        {
            var pupilRep = new PupilRep();
            WyborPupila.ItemsSource = pupilRep.GetByUzytkownik(Helpers.SessionHelper.ZalogowanyUzytkownik!.IdUzytkownik);

            var zabiegRep = new ZabiegRep();
            ListaZabiegow.ItemsSource = zabiegRep.GetAll();
        }

        private void WybierzZabieg(object sender, MouseButtonEventArgs e)
        {
            foreach (var item in ListaZabiegow.Items)
            {
                var container = ListaZabiegow.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;

                if (container != null)
                {
                    var border = FindChild<Border>(container);
                    if (border != null) border.BorderBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFE5E7EB"));
                }
            }

            var clicked = sender as Border;

            if (clicked != null)
            {
                clicked.BorderBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFB7185"));
                _wybranyZabieg = clicked.DataContext as Models.Zabieg;
            }
        }

        private void WybierzWeterynarza(object sender, MouseButtonEventArgs e)
        {
            foreach (var item in ListaWeterynarzy.Items)
            {
                var container = ListaWeterynarzy.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;

                if (container != null)
                {
                    var border = FindChild<Border>(container);
                    if (border != null) border.BorderBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFE5E7EB"));
                }
            }

            var clickedBorder = sender as Border;

            if (clickedBorder != null)
            {
                clickedBorder.BorderBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFB7185"));
                _wybranyWeterynarz = clickedBorder.DataContext as Models.Weterynarz;
            }
        }

        private void Dalej_Click(object sender, RoutedEventArgs e)
        {
            if (_aktualnyKrok == 1)
            {
                if (WyborPupila.SelectedItem == null)
                {
                    System.Windows.MessageBox.Show("Musisz wybrać pupila!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (_wybranyZabieg == null)
                {
                    System.Windows.MessageBox.Show("Musisz wybrać zabieg!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                _wybranyPupil = WyborPupila.SelectedItem as Models.Pupil;

                var wetRep = new WeterynarzRep();
                ListaWeterynarzy.ItemsSource = wetRep.GetByZabieg(_wybranyZabieg.IdZabieg);

                Krok1.Visibility = Visibility.Collapsed;
                Krok2.Visibility = Visibility.Visible;
                BtnWstecz.Visibility = Visibility.Visible;
                _aktualnyKrok = 2;
            }
            else if (_aktualnyKrok == 2)
            {
                if (_wybranyWeterynarz == null)
                {
                    System.Windows.MessageBox.Show("Musisz wybrać weterynarza!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                WygenerujKalendarz();

                Krok2.Visibility = Visibility.Collapsed;
                Krok3.Visibility = Visibility.Visible;
                BtnDalej.Content = "Zarezerwuj";
                _aktualnyKrok = 3;
            }
            else if (_aktualnyKrok == 3)
            {
                if (_wybranaData == default || _wybranaGodzina == default)
                {
                    System.Windows.MessageBox.Show("Musisz wybrać termin!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                ZapiszRezerwacje();
            }
        }

        private void Wstecz_Click(object sender, RoutedEventArgs e)
        {
            if (_aktualnyKrok == 2)
            {
                Krok2.Visibility = Visibility.Collapsed;
                Krok1.Visibility = Visibility.Visible;
                BtnWstecz.Visibility = Visibility.Collapsed;
                _aktualnyKrok = 1;
            }
            else if (_aktualnyKrok == 3)
            {
                Krok3.Visibility = Visibility.Collapsed;
                Krok2.Visibility = Visibility.Visible;
                BtnDalej.Content = "Dalej";
                _aktualnyKrok = 2;
            }
        }

        private void WygenerujKalendarz()
        {
            KalendarzGrid.Children.Clear();
            KalendarzGrid.RowDefinitions.Clear();
            KalendarzGrid.ColumnDefinitions.Clear();

            var rep = new RezerwacjaRep();
            var dostepnoscRep = new DostepnoscRep();
            var dostepnosc = dostepnoscRep.GetByWeterynarz(_wybranyWeterynarz!.IdWeterynarz);

            KalendarzGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
            var startDate = DateTime.Today;

            var dni = new List<DateTime>();
            for (int i = 0; i < 7; i++)
            {
                var dzien = startDate.AddDays(i);
                int dzienISO = (int)dzien.DayOfWeek == 0 ? 7 : (int)dzien.DayOfWeek;
                if (dostepnosc.Any(d => d.DzienTygodnia == dzienISO)) dni.Add(dzien);
            }

            foreach (var dzien in dni)
                KalendarzGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            KalendarzGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var nazwyDni = new[] { "Pon", "Wt", "Śr", "Czw", "Pt", "Sob", "Niedz" };

            for (int col = 0; col < dni.Count; col++)
            {
                var header = new TextBlock
                {
                    Text = $"{nazwyDni[(int)dni[col].DayOfWeek == 0 ? 6 : (int)dni[col].DayOfWeek - 1]} {dni[col]:d.MM}",
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(4, 0, 4, 8)
                };

                Grid.SetRow(header, 0);
                Grid.SetColumn(header, col + 1);
                KalendarzGrid.Children.Add(header);
            }

            var rezerwacje = rep.GetByWeterynarz(_wybranyWeterynarz.IdWeterynarz);

            for (int godz = 8; godz <= 16; godz++)
            {
                var rowIndex = godz - 7;
                KalendarzGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var godzLabel = new TextBlock
                {
                    Text = $"{godz}:00",
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 4, 8, 4),
                    Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF9CA3AF"))
                };

                Grid.SetRow(godzLabel, rowIndex);
                Grid.SetColumn(godzLabel, 0);
                KalendarzGrid.Children.Add(godzLabel);

                for (int col = 0; col < dni.Count; col++)
                {
                    var dzien = dni[col];
                    var godzStart = new TimeSpan(godz, 0, 0);
                    var godzKoniec = godzStart.Add(TimeSpan.FromMinutes(_wybranyZabieg!.CzasTrwaniaMin));

                    int dzienISO = (int)dzien.DayOfWeek == 0 ? 7 : (int)dzien.DayOfWeek;
                    var dostepnoscDnia = dostepnosc.FirstOrDefault(d => d.DzienTygodnia == dzienISO);

                    bool czyWPracy = dostepnoscDnia != null && godzStart >= dostepnoscDnia.GodzStart && godzKoniec <= dostepnoscDnia.GodzKoniec;
                    bool czyZajety = rezerwacje.Any(r => r.DataRezerwacji.Date == dzien.Date && r.Godzina_Start < godzKoniec && r.Godzina_Koniec > godzStart);

                    var button = new System.Windows.Controls.Button
                    {
                        Content = czyZajety ? "Zajęte" : (czyWPracy ? "Wolne" : "-"),
                        Margin = new Thickness(4),
                        Height = 36,
                        BorderThickness = new Thickness(0),
                        FontWeight = FontWeights.Bold,
                        FontSize = 12,
                        IsEnabled = czyWPracy && !czyZajety
                    };

                    button.Background = czyZajety
                    ? new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFEE2E2"))
                    : czyWPracy
                    ? new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFE6F4EA"))
                    : new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFF3F4F6"));

                    button.Foreground = czyZajety
                    ? new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFE53E3E"))
                    : czyWPracy
                    ? new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF2E7D32"))
                    : new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF9CA3AF"));

                    var template = new ControlTemplate(typeof(System.Windows.Controls.Button));
                    var border = new FrameworkElementFactory(typeof(Border));

                    border.SetValue(Border.CornerRadiusProperty, new CornerRadius(8));
                    border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(System.Windows.Controls.Button.BackgroundProperty));

                    var presenter = new FrameworkElementFactory(typeof(ContentPresenter));

                    presenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, System.Windows.HorizontalAlignment.Center);
                    presenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                    border.AppendChild(presenter);
                    template.VisualTree = border;
                    button.Template = template;

                    var capturedDzien = dzien;
                    var capturedGodz = godzStart;
                    button.Click += (s, e) =>
                    {
                        if (_wybranySlot != null)
                        {
                            _wybranySlot.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFE6F4EA"));
                            _wybranySlot.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF2E7D32"));
                            _wybranySlot.Content = "Wolne";
                        }

                        var klikniety = s as System.Windows.Controls.Button;
                        klikniety!.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFB7185"));
                        klikniety.Foreground = System.Windows.Media.Brushes.White;
                        klikniety.Content = "Wybrano";
                        _wybranySlot = klikniety;
                        _wybranaData = capturedDzien;
                        _wybranaGodzina = capturedGodz;
                    };

                    Grid.SetRow(button, rowIndex);
                    Grid.SetColumn(button, col + 1);
                    KalendarzGrid.Children.Add(button);
                }
            }
        }

        private void ZapiszRezerwacje()
        {
            try
            {
                var rezerwacja = new Models.Rezerwacja
                {
                    IdWeterynarz = _wybranyWeterynarz!.IdWeterynarz,
                    IdPupil = _wybranyPupil!.IdPupil,
                    IdZabieg = _wybranyZabieg!.IdZabieg,
                    IdUzytkownik = Helpers.SessionHelper.ZalogowanyUzytkownik!.IdUzytkownik,
                    DataRezerwacji = _wybranaData,
                    Godzina_Start = _wybranaGodzina,
                    Godzina_Koniec = _wybranaGodzina.Add(TimeSpan.FromMinutes(_wybranyZabieg.CzasTrwaniaMin)),
                    Status = "Zarezerwowany"
                };

                var rep = new RezerwacjaRep();
                rep.Dodaj(rezerwacja);

                System.Windows.MessageBox.Show("Rezerwacja została zapisana!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas zapisu rezerwacji: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static T? FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t) return t;
                var result = FindChild<T>(child);
                if (result != null) return result;
            }

            return null;
        }
    }
}