    // Metoda Dalej_Click

    if (_aktualnyKrok == 1)
    {
        if (WyborPupila.SelectedItem == null)
        {
            System.Windows.MessageBox.Show("Musisz wybrać pupila!", 
            "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (_wybranyZabieg == null)
        {
            System.Windows.MessageBox.Show("Musisz wybrać zabieg!", 
            "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var saldo = Helpers.SessionHelper.ZalogowanyUzytkownik!.Saldo;
        if (saldo < (int)_wybranyZabieg.Cena)
        {
            System.Windows.MessageBox.Show(
                $"Niewystarczające środki na koncie!\n" +
                $"Koszt zabiegu: {_wybranyZabieg.Cena:F2} zł\n" +
                $"Twoje saldo: {saldo} zł", 
                "Brak środków", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        _wybranyPupil = WyborPupila.SelectedItem as Models.Pupil;
        var wetRep = new WeterynarzRep();
        ListaWeterynarzy.ItemsSource = 
        wetRep.GetByZabieg(_wybranyZabieg.IdZabieg);

        Krok1.Visibility = Visibility.Collapsed;
        Krok2.Visibility = Visibility.Visible;
        BtnWstecz.Visibility = Visibility.Visible;
        _aktualnyKrok = 2;
    }
    else if (_aktualnyKrok == 2)
    {
        if (_wybranyWeterynarz == null)
        {
            System.Windows.MessageBox.Show("Musisz wybrać weterynarza!", 
            "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            System.Windows.MessageBox.Show("Musisz wybrać termin!", 
            "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        ZapiszRezerwacje();
    }

    // Metoda GetByZabieg w klasie WeterynarzRep

    var lista = new List<Weterynarz>();

    using var Conn = Database.GetConnection();
    Conn.Open();

    var command = new SqlCommand("SELECT w.* FROM Weterynarz w " +
    "INNER JOIN Weterynarz_Zabieg wz " + 
    "ON w.id_weterynarz = wz.id_weterynarz WHERE wz.id_zabieg = @id", Conn);
    command.Parameters.AddWithValue("@id", idZabieg);

    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        lista.Add(MapujWeterynarza(reader));
    }

     return lista;