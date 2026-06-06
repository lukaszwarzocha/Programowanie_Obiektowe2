    // Metoda DodajWeterynarza_Click w DodajWeterynarzaWindow

    var imie = PoleImie.Text;
    var nazwisko = PoleNazwisko.Text;
    var specjalizacja = PoleSpecjalizacja.Text;
    var email = PoleEmail.Text;
    var telefon = PoleTelefon.Text;
    var haslo = PoleHaslo.Password;

    if (string.IsNullOrEmpty(imie) || string.IsNullOrEmpty(nazwisko) ||
        string.IsNullOrEmpty(email) || string.IsNullOrEmpty(haslo))
    {
        System.Windows.MessageBox.Show("Musisz wypełnić wymagane pola" +
        "(imię, nazwisko, email, hasło)!", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        System.Windows.MessageBox.Show("Weterynarz został dodany pomyślnie!", 
        "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        this.Close();
    }
    catch (Exception ex)
    {
        System.Windows.MessageBox.Show($"Błąd: {ex.Message}", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
    }

   // Metoda Dodaj w WeterynarzRep

    using var Conn = Database.GetConnection();
    Conn.Open();

    var command = new SqlCommand(
        "INSERT INTO Weterynarz (imie, nazwisko, telefon, specjalizacja, email, haslo) " +
        "VALUES (@imie, @nazwisko, @telefon, @specjalizacja, @email, @haslo)", Conn);
    command.Parameters.AddWithValue("@imie", w.Imie);
    command.Parameters.AddWithValue("@nazwisko", w.Nazwisko);
    command.Parameters.AddWithValue("@telefon", w.Telefon);
    command.Parameters.AddWithValue("@specjalizacja", w.Specjalizacja);
    command.Parameters.AddWithValue("@email", w.Email);
    command.Parameters.AddWithValue("@haslo", w.Haslo);
    command.ExecuteNonQuery();

    // Metoda ZapiszDostepnosc w DodajWeterynarzaWindow

    if (string.IsNullOrWhiteSpace(od) || string.IsNullOrWhiteSpace(doo)) return;

    if (!TimeSpan.TryParse(od, out var gOd) 
    || !TimeSpan.TryParse(doo, out var gDo) || gOd >= gDo) return;

    rep.Dodaj(new Models.Dostepnosc
    {
        IdWeterynarz = idWet,
        DzienTygodnia = dzien,
        GodzStart = gOd,
        GodzKoniec = gDo
    });

    // Metoda Dodaj w DostepnoscRep

     using var Conn = Database.GetConnection();
     Conn.Open();

     var command = new SqlCommand("INSERT INTO Dostepnosc " +
     "(id_weterynarz, dzien_tygodnia, godzina_start, godzina_koniec)" +
     "VALUES (@id, @dzien, @od, @do)", Conn);
     command.Parameters.AddWithValue("@id", d.IdWeterynarz);
     command.Parameters.AddWithValue("@dzien", d.DzienTygodnia);
     command.Parameters.AddWithValue("@od", d.GodzStart);
     command.Parameters.AddWithValue("@do", d.GodzKoniec);
     command.ExecuteNonQuery();
 