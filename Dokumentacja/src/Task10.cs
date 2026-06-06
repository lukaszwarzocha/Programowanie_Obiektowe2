    // Metoda Dodaj_Click w DodajZabiegWindow

    var nazwa = PoleNazwa.Text;
    var czasTxt = PoleCzas.Text;
    var cenaTxt = PoleCena.Text;

    if (string.IsNullOrEmpty(nazwa) 
    || string.IsNullOrEmpty(czasTxt) || string.IsNullOrEmpty(cenaTxt))
    {
        System.Windows.MessageBox.Show("Musisz wypełnić wszystkie pola!", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    if (!int.TryParse(czasTxt, out int czas) || czas <= 0)
    {
        System.Windows.MessageBox.Show("Musisz podać prawidłowy" +
        "czas trwania (minuty)!", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    if (!decimal.TryParse(cenaTxt, out decimal cena) || cena < 0)
    {
        System.Windows.MessageBox.Show("Musisz podać prawidłową cenę!", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        System.Windows.MessageBox.Show("Zabieg został dodany pomyślnie!", 
        "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        this.Close();
    }
    catch (Exception ex)
    {
        System.Windows.MessageBox.Show($"Błąd: {ex.Message}", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
    }

      // Metoda Dodaj w ZabiegRep

      using var Conn = Database.GetConnection();
      Conn.Open();

      var coomand = new SqlCommand("INSERT INTO Zabieg  " +
      "(nazwa, opis, cena, czas_trwania_min) VALUES" +
      "(@nazwa, @opis, @cena, @czas)", Conn);
      coomand.Parameters.AddWithValue("@nazwa", z.Nazwa);
      coomand.Parameters.AddWithValue("@opis", (object?)z.Opis ?? DBNull.Value);
      coomand.Parameters.AddWithValue("@cena", z.Cena);
      coomand.Parameters.AddWithValue("@czas", z.CzasTrwaniaMin);
      coomand.ExecuteNonQuery();
  

    // Metoda UsunZabieg_Click w panelu administratora

    var button = sender as System.Windows.Controls.Button;
    var id = (int)button!.Tag;

    var potwierdzenie = System.Windows.MessageBox.Show("Usunięcie zabiegu usunie " +
    "też powiązane rezerwacje. Kontynuować?",
    "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
    if (potwierdzenie == MessageBoxResult.No) return;

    try
    {
        var rep = new ZabiegRep();
        rep.Usun(id);
        WczytajZabiegi();
    }
    catch (Exception ex)
    {
        System.Windows.MessageBox.Show($"Błąd usuwania: {ex.Message}", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
    }

      // Metoda Usun w ZabiegRep

      using var Conn = Database.GetConnection();
      Conn.Open();

      var coomandRez = new SqlCommand("DELETE FROM Rezerwacja" +
      "WHERE id_zabieg = @id", Conn);
      coomandRez.Parameters.AddWithValue("@id", idZabieg);
      coomandRez.ExecuteNonQuery();

      var coomandWZ = new SqlCommand("DELETE FROM Weterynarz_Zabieg" +
      "WHERE id_zabieg = @id", Conn);
      coomandWZ.Parameters.AddWithValue("@id", idZabieg);
      coomandWZ.ExecuteNonQuery();

      var coomand = new SqlCommand("DELETE FROM Zabieg" +
      "WHERE id_zabieg = @id", Conn);
      coomand.Parameters.AddWithValue("@id", idZabieg);
      coomand.ExecuteNonQuery();
  