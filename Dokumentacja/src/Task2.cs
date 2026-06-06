
    // Metoda Zmien haslo w panelu użytkownika
    
    var uzytkownik = Helpers.SessionHelper.ZalogowanyUzytkownik;
    if (uzytkownik == null) return;

    var aktualne = AktualneHaslo.Password;
    var nowe = NoweHaslo.Password;
    var potwierdz_haslo = PowtorzHaslo.Password;

    if (string.IsNullOrEmpty(aktualne) || string.IsNullOrEmpty(nowe) 
    || string.IsNullOrEmpty(potwierdz_haslo))
    {
        System.Windows.MessageBox.Show("Pierw musisz wypełnić wszystkie " + 
        "pola dotyczące hasła!", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    if (aktualne != uzytkownik.Haslo)
    {
        System.Windows.MessageBox.Show("Obecne hasło jest nieprawidłowe!", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    if (nowe != potwierdz_haslo)
    {
        System.Windows.MessageBox.Show("Nowe hasło musi być takie same " + 
        "jak w polu potwierdzającym!", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    if (nowe.Length < 6)
    {
        System.Windows.MessageBox.Show("Hasło musi mieć minimum 6 znaków!", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    var potwierdz = System.Windows.MessageBox.Show("Czy na pewno chcesz zmienić hasło?", 
    "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
    if (potwierdz == MessageBoxResult.No) return;

    try
    {
        uzytkownik.Haslo = nowe;
        var rep = new Repositories.UzytkownikRep();
        rep.Zmiana_hasla(uzytkownik);

        AktualneHaslo.Clear(); NoweHaslo.Clear(); PowtorzHaslo.Clear();

        System.Windows.MessageBox.Show("Twoje hasło zostało zmienione!", 
        "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    catch (Exception ex)
    {
        System.Windows.MessageBox.Show($"Błąd podczas zmiany hasła: {ex.Message}", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    // Zmiana_hasla w UzytkownikRep

    using var Conn = Database.GetConnection();
    Conn.Open();

    var command = new SqlCommand("Update Uzytkownik SET haslo = " +
    "@haslo WHERE id_uzytkownik = @id", Conn);
    command.Parameters.AddWithValue("@haslo", user.Haslo);
    command.Parameters.AddWithValue("@id", user.IdUzytkownik);

    command.ExecuteNonQuery();

