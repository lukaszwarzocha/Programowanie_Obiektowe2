    // WczytajUzytkownikow

    try
    {
        var rep = new UzytkownikRep();
        ListaUzytkownikow.ItemsSource = rep.GetAllUzytkownicy();
    }
    catch (Exception ex)
    {
        System.Windows.MessageBox.Show($"Błąd: {ex.Message}", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    // Metoda GetAllUzytkownicy w UzytkownikRep

    var lista = new List<Uzytkownik>();

    using var Conn = Database.GetConnection();
    Conn.Open();

    var command = new SqlCommand(
        "SELECT * FROM Uzytkownik WHERE rola = 'Uzytkownik' " +
        "ORDER BY nazwisko", Conn);
    using var reader = command.ExecuteReader();

    while (reader.Read())
    {
        lista.Add(new Uzytkownik
        {
            IdUzytkownik = (int)reader["id_uzytkownik"],
            Imie = reader["imie"].ToString()!,
            Nazwisko = reader["nazwisko"].ToString()!,
            Email = reader["email"].ToString()!,
            Telefon = reader["telefon"].ToString()!,
            Saldo = (int)reader["Saldo"]
        });
    }

    // return lista;

    // Metoda UsunUzytkownika_Click

    var button = sender as System.Windows.Controls.Button;
    var id = (int)button!.Tag;

    var potwierdzenie = System.Windows.MessageBox.Show("Usunięcie użytkownika usunie też " +
    "jego pupile i rezerwacje. Kontynuować?",
    "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
    if (potwierdzenie == MessageBoxResult.No) return;

    try
    {
        var rep = new UzytkownikRep();
        rep.UsunUzytkownika(id);
        WczytajUzytkownikow();
    }
    catch (Exception ex)
    {
        System.Windows.MessageBox.Show($"Błąd usuwania: {ex.Message}", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    // Metoda UsunUzytkownika w UzytkownikRep
    
    using var Conn = Database.GetConnection();
    Conn.Open();

    var coomandRez = new SqlCommand("DELETE FROM Rezerwacja" +
    "WHERE id_uzytkownik = @id", Conn);
    coomandRez.Parameters.AddWithValue("@id", idUzytkownik);
    coomandRez.ExecuteNonQuery();

    var coomandPupile = new SqlCommand("SELECT id_pupil FROM" +
    "Uzytkownik_Pupil WHERE id_uzytkownik = @id", Conn);
    coomandPupile.Parameters.AddWithValue("@id", idUzytkownik);
    var idPupili = new List<int>();
    using (var reader = coomandPupile.ExecuteReader())
        while (reader.Read()) idPupili.Add((int)reader["id_pupil"]);

    var coomandUP = new SqlCommand("DELETE FROM Uzytkownik_Pupil" +
    "WHERE id_uzytkownik = @id", Conn);
    coomandUP.Parameters.AddWithValue("@id", idUzytkownik);
    coomandUP.ExecuteNonQuery();

    foreach (var idPupil in idPupili)
    {
        var coomandCheck = new SqlCommand("SELECT COUNT(*) FROM" +
        "Uzytkownik_Pupil WHERE id_pupil = @id", Conn);
        coomandCheck.Parameters.AddWithValue("@id", idPupil);
        int count = (int)coomandCheck.ExecuteScalar();
        if (count == 0)
        {
            var coomanddDelPupil = new SqlCommand("DELETE FROM Pupil" +
            "WHERE id_pupil = @id", Conn);
            coomanddDelPupil.Parameters.AddWithValue("@id", idPupil);
            coomanddDelPupil.ExecuteNonQuery();
        }
    }

    var coomandUser = new SqlCommand("DELETE FROM Uzytkownik" +
    "WHERE id_uzytkownik = @id", Conn);
    coomandUser.Parameters.AddWithValue("@id", idUzytkownik);
    coomandUser.ExecuteNonQuery();

