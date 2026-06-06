    //Metoda Zaloguj_sie

    var email = PoleEmail.Text;
    var haslo = PoleHaslo.Password;

    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(haslo))
    {
        System.Windows.MessageBox.Show("Nieprawidłowy email lub hasło!", "Błąd", 
        MessageBoxButton.OK, MessageBoxImage.Error);
        return;
    }

    try
    {
        var rep = new UzytkownikRep();
        var uzytkownik = rep.GetUser(email, haslo);

        if (uzytkownik == null)
        {
            System.Windows.MessageBox.Show("Nieprawidłowy login lub hasło!", "Błąd", 
            MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        Helpers.SessionHelper.ZalogowanyUzytkownik = uzytkownik; 

        if (uzytkownik.Rola == "Administrator")
        {
            new AdminWindow().Show();
        }
        else
        {
            new MainWindow().Show();
        }
        this.Close();
    }
    catch (Exception ex)
    {
        System.Windows.MessageBox.Show($"Nie udało ci się połączyć z bazą danych: {ex.Message}", 
        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    //Helper SessionHelper
    public static Models.Uzytkownik? ZalogowanyUzytkownik { get; set; }

    //GetUser w UzytkownikRep
    
    using var Conn = Database.GetConnection();
    Conn.Open();

    var command = new SqlCommand("SELECT * FROM Uzytkownik WHERE email = @email " +
    "AND haslo = @haslo", Conn);
    command.Parameters.AddWithValue("@email", email);
    command.Parameters.AddWithValue("@haslo", password);

    using var reader = command.ExecuteReader();
    if (reader.Read())
    {
        return new Uzytkownik
        {
            IdUzytkownik = (int)reader["id_uzytkownik"],
            Imie = reader["imie"].ToString()!,
            Nazwisko = reader["nazwisko"].ToString()!,
            Email = reader["email"].ToString()!,
            Telefon = reader["telefon"].ToString()!,
            Adres = reader["adres"].ToString()!,
            Haslo = reader["haslo"].ToString()!,
            Rola = reader["rola"].ToString()!,
            Saldo = (int)reader["Saldo"]!
        };
    }

    return null;
