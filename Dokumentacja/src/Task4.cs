    // Metoda WplacSrodki_Click

    var kwotaT = PoleKwota.Text;

    if (!int.TryParse(kwotaT, out int kwota) || kwota <= 0)
    {
        System.Windows.MessageBox.Show("Musisz podać prawidłową kwotę!",
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    var potwierdzenie = System.Windows.MessageBox.Show("Czy na pewno chcesz " +
    $"wpłacić {kwota} zł?", 
    "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
    if (potwierdzenie == MessageBoxResult.No) return;

    try
    {
        var rep = new Repositories.UzytkownikRep();
        rep.DoladujSaldo(Helpers.SessionHelper.ZalogowanyUzytkownik!.IdUzytkownik, kwota);

        Helpers.SessionHelper.ZalogowanyUzytkownik.Saldo += kwota;
        EtykietaSaldoUzytkownik.Content = 
        $"Saldo: {Helpers.SessionHelper.ZalogowanyUzytkownik.Saldo} zł";
        PoleKwota.Clear();

        System.Windows.MessageBox.Show($"Wpłata kwoty {kwota} zł" + 
        "przebiegła pomyślnie!", 
        "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    catch (Exception ex)
    {
        System.Windows.MessageBox.Show($"Błąd: {ex.Message}", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    // Metoda DoladujSaldo w UzytkownikRep

   {
     using var Conn = Database.GetConnection();
     Conn.Open();

     var command = new SqlCommand("UPDATE Uzytkownik SET Saldo = " +
     "Saldo + @kwota WHERE id_uzytkownik = @id", Conn);
     command.Parameters.AddWithValue("@kwota", kwota);
     command.Parameters.AddWithValue("@id", idUzytkownik);
     command.ExecuteNonQuery();
   }