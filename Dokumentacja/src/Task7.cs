    // Metoda WygenerujKalendarz

    KalendarzGrid.Children.Clear();
    KalendarzGrid.RowDefinitions.Clear();
    KalendarzGrid.ColumnDefinitions.Clear();

    var rep = new RezerwacjaRep();
    var dostepnoscRep = new DostepnoscRep();
    var dostepnosc = dostepnoscRep.GetByWeterynarz(_wybranyWeterynarz!.IdWeterynarz);

    KalendarzGrid.ColumnDefinitions.Add(new ColumnDefinition { 
        Width = new GridLength(60) });
    var startDate = DateTime.Today;

    var dni = new List<DateTime>();
    for (int i = 0; i < 7; i++)
    {
        var dzien = startDate.AddDays(i);
        int dzienISO = (int)dzien.DayOfWeek == 0 ? 7 : (int)dzien.DayOfWeek;
        if (dostepnosc.Any(d => d.DzienTygodnia == dzienISO)) dni.Add(dzien);
    }

    foreach (var dzien in dni)
        KalendarzGrid.ColumnDefinitions.Add(new ColumnDefinition { 
            Width = new GridLength(1, GridUnitType.Star) });

    KalendarzGrid.RowDefinitions.Add(new RowDefinition { 
    Height = GridLength.Auto });

    var nazwyDni = new[] { "Pon", "Wt", "Śr", "Czw", "Pt", "Sob", "Niedz" };

    for (int col = 0; col < dni.Count; col++)
    {
        var header = new TextBlock
        {
            Text = 
            $"{nazwyDni[(int)dni[col].DayOfWeek == 0 ? 6 : (int)dni[col].DayOfWeek - 1]} {dni[col]:d.MM}",
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
            Foreground = new SolidColorBrush((System.Windows.Media.Color)
            System.Windows.Media.ColorConverter.ConvertFromString("#FF9CA3AF"))
        };

        Grid.SetRow(godzLabel, rowIndex);
        Grid.SetColumn(godzLabel, 0);
        KalendarzGrid.Children.Add(godzLabel);

        for (int col = 0; col < dni.Count; col++)
        {
            var dzien = dni[col];
            var godzStart = 
            new TimeSpan(godz, 0, 0);
            var godzKoniec = 
            godzStart.Add(TimeSpan.FromMinutes(_wybranyZabieg!.CzasTrwaniaMin));

            int dzienISO = (int)dzien.DayOfWeek == 0 ? 7 : (int)dzien.DayOfWeek;
            var dostepnoscDnia = 
            dostepnosc.FirstOrDefault(d => d.DzienTygodnia == dzienISO);

            bool czyWPracy = dostepnoscDnia != null 
            && godzStart >= dostepnoscDnia.GodzStart && 
            godzKoniec <= dostepnoscDnia.GodzKoniec;
            bool czyZajety = rezerwacje.Any(r => 
            r.DataRezerwacji.Date == dzien.Date && r.Godzina_Start < godzKoniec && 
            r.Godzina_Koniec > godzStart);

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
            ? new SolidColorBrush((System.Windows.Media.Color)
            System.Windows.Media.ColorConverter.ConvertFromString("#FFFEE2E2"))
            : czyWPracy
            ? new SolidColorBrush((System.Windows.Media.Color)
            System.Windows.Media.ColorConverter.ConvertFromString("#FFE6F4EA"))
            : new SolidColorBrush((System.Windows.Media.Color)
            System.Windows.Media.ColorConverter.ConvertFromString("#FFF3F4F6"));

            button.Foreground = czyZajety
            ? new SolidColorBrush((System.Windows.Media.Color)
            System.Windows.Media.ColorConverter.ConvertFromString("#FFE53E3E"))
            : czyWPracy
            ? new SolidColorBrush((System.Windows.Media.Color)
            System.Windows.Media.ColorConverter.ConvertFromString("#FF2E7D32"))
            : new SolidColorBrush((System.Windows.Media.Color)
            System.Windows.Media.ColorConverter.ConvertFromString("#FF9CA3AF"));

            var template = new ControlTemplate(typeof(System.Windows.Controls.Button));
            var border = new FrameworkElementFactory(typeof(Border));

            border.SetValue(Border.CornerRadiusProperty, new CornerRadius(8));
            border.SetValue(Border.BackgroundProperty, 
            new TemplateBindingExtension(System.Windows.Controls.Button.BackgroundProperty));

            var presenter = new FrameworkElementFactory(typeof(ContentPresenter));

            presenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, 
            System.Windows.HorizontalAlignment.Center);
            presenter.SetValue(ContentPresenter.VerticalAlignmentProperty, 
            VerticalAlignment.Center);
            border.AppendChild(presenter);
            template.VisualTree = border;
            button.Template = template;

            var capturedDzien = dzien;
            var capturedGodz = godzStart;
            button.Click += (s, e) =>
            {
                if (_wybranySlot != null)
                {
                    _wybranySlot.Background = new SolidColorBrush((System.Windows.Media.Color)
                    System.Windows.Media.ColorConverter.ConvertFromString("#FFE6F4EA"));
                    _wybranySlot.Foreground = new SolidColorBrush((System.Windows.Media.Color)
                    System.Windows.Media.ColorConverter.ConvertFromString("#FF2E7D32"));
                    _wybranySlot.Content = "Wolne";
                }

                var klikniety = s as System.Windows.Controls.Button;
                klikniety!.Background = new SolidColorBrush((System.Windows.Media.Color)
                System.Windows.Media.ColorConverter.ConvertFromString("#FFFB7185"));
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

    // Metoda GetByWeterynarz w DostepnoscRep
    
    var lista = new List<Dostepnosc>();

    using var Conn = Database.GetConnection();
    Conn.Open();

    var command = new SqlCommand("SELECT * FROM Dostepnosc" +
    "WHERE id_weterynarz = @id", Conn);
    command.Parameters.AddWithValue("@id", idWeterynarz);

    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        lista.Add(new Dostepnosc
        {
            IdGodziny = (int)reader["id_godziny"],
            IdWeterynarz = (int)reader["id_weterynarz"],
            DzienTygodnia = (int)reader["dzien_tygodnia"],
            GodzStart = (TimeSpan)reader["godzina_start"],
            GodzKoniec = (TimeSpan)reader["godzina_koniec"]
        });
    }

    return lista;
