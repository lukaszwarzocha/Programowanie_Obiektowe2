    // Metoda WczytajRezerwacje w panelu administratora

    try
    {
        var rep = new RezerwacjaRep();
        _wszystkieRezerwacje = rep.GetAllAdmin();
        ZastosujFiltry();
    }
    catch (Exception ex)
    {
        System.Windows.MessageBox.Show($"Błąd: {ex.Message}", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
    }

     // Metoda GetAllAdmin w RezerwacjaRep

     var lista = new List<RezerwacjaAdmin>();

     using var Conn = Database.GetConnection();
     Conn.Open();

     var command = new SqlCommand(
         "SELECT r.id_rezerwacja, r.id_weterynarz, r.data_rezerwacji, " +
         "r.godzina_start, r.godzina_koniec, r.status, " +
         "w.imie + ' ' + w.nazwisko AS imie_nazwisko_weterynarza, " +
         "p.imie AS imie_pupila, " +
         "z.nazwa AS nazwa_zabiegu, " +
         "u.imie + ' ' + u.nazwisko AS imie_nazwisko_uzytkownika " +
         "FROM Rezerwacja r " +
         "INNER JOIN Weterynarz w ON r.id_weterynarz = w.id_weterynarz " +
         "INNER JOIN Pupil p ON r.id_pupil = p.id_pupil " +
         "INNER JOIN Zabieg z ON r.id_zabieg = z.id_zabieg " +
         "INNER JOIN Uzytkownik u ON r.id_uzytkownik = u.id_uzytkownik " +
         "ORDER BY r.data_rezerwacji DESC, r.godzina_start", Conn);

     using var reader = command.ExecuteReader();
     while (reader.Read())
     {
         lista.Add(new RezerwacjaAdmin
         {
             IdRezerwacja = (int)reader["id_rezerwacja"],
             IdWeterynarz = (int)reader["id_weterynarz"],
             DataRezerwacji = (DateTime)reader["data_rezerwacji"],
             GodzStart = (TimeSpan)reader["godzina_start"],
             GodzKoniec = (TimeSpan)reader["godzina_koniec"],
             Status = reader["status"].ToString()!,
             ImieNazwiskoWeterynarza = 
             reader["imie_nazwisko_weterynarza"].ToString()!,
             ImiePupila = reader["imie_pupila"].ToString()!,
             NazwaZabiegu = reader["nazwa_zabiegu"].ToString()!,
             ImieNazwiskoUzytkownika = 
             reader["imie_nazwisko_uzytkownika"].ToString()!
         });
     }

    // return lista;
 
    // Metoda Filtry_Changed w panelu administratora

    if (_inicjalizacja) return; 
    ZastosujFiltry();


    // Metoda ZastosujFiltry w panelu administratora
    
    if (_inicjalizacja) return;
    if (_wszystkieRezerwacje == null) return;
    if (ListaRezerwacji == null) return;

    var wynik = _wszystkieRezerwacje.AsEnumerable();

    if (FiltrData.SelectedDate.HasValue) 
    {
        wynik = 
        wynik.Where(r => r.DataRezerwacji.Date == FiltrData.SelectedDate.Value.Date);
    }

    if (FiltrWeterynarz.SelectedItem is Weterynarz wet && wet.IdWeterynarz != 0) 
    {
        wynik = wynik.Where(r => r.IdWeterynarz == wet.IdWeterynarz);
    }

    if (FiltrStatus.SelectedItem is ComboBoxItem statusItem 
    && statusItem.Content?.ToString() != "Wszystkie statusy")
        wynik = 
        wynik.Where(r => r.Status == statusItem.Content?.ToString());

    ListaRezerwacji.ItemsSource = wynik.ToList();

    // Metoda WyczyscFiltry_Click

    FiltrData.SelectedDate = null;
    if (FiltrWeterynarz.Items.Count > 0) FiltrWeterynarz.SelectedIndex = 0;
    FiltrStatus.SelectedIndex = 0;
    ZastosujFiltry();


    // Metoda StatusRezerwacji_Changed w panelu administratora

    var combo = sender as System.Windows.Controls.ComboBox;
    if (combo?.Tag == null) return;

    var idRezerwacja = (int)combo.Tag;
    var nowyStatus = (combo.SelectedItem as ComboBoxItem)?.Content?.ToString();
    if (nowyStatus == null) return;

    try
    {
        var rep = new RezerwacjaRep();
        rep.ZmienStatus(idRezerwacja, nowyStatus);
    }
    catch (Exception ex)
    {
        System.Windows.MessageBox.Show($"Błąd zmiany statusu: {ex.Message}", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    // Metoda ZmienStatus w RezerwacjaRep

      using var Conn = Database.GetConnection();
      Conn.Open();

      var command = new SqlCommand(
          "UPDATE Rezerwacja SET status = @status " +
          "WHERE id_rezerwacja = @id", Conn);
      command.Parameters.AddWithValue("@status", nowyStatus);
      command.Parameters.AddWithValue("@id", idRezerwacja);
      command.ExecuteNonQuery();
  
    // Metoda AktualizujPrzeterminowane W App.xaml.cs

      try
      {
          using var Conn = Database.GetConnection();
          Conn.Open();

          var command = new SqlCommand("UPDATE Rezerwacja SET status = " +
          "'Zrealizowany' WHERE status = 'Zarezerwowany' " +
          "AND CAST(data_rezerwacji AS DATETIME) + CAST(godzina_koniec AS DATETIME)" +
          "< GETDATE()", Conn);
          command.ExecuteNonQuery();
      }
      catch { }
  