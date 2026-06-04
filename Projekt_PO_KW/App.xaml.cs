using Microsoft.Data.SqlClient;
using System.Windows;
using Application = System.Windows.Application;

namespace Projekt_PO_KW
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AktualizujPrzeterminowane();
            new Views.LoginWindow().Show();
        }
        private void AktualizujPrzeterminowane()
        {
            try
            {
                using var Conn = Database.GetConnection();
                Conn.Open();

                var command = new SqlCommand("UPDATE Rezerwacja SET status = 'Zrealizowany' WHERE status = 'Zarezerwowany' " +
                    "AND CAST(data_rezerwacji AS DATETIME) + CAST(godzina_koniec AS DATETIME) < GETDATE()", Conn);
                command.ExecuteNonQuery();
            }
            catch { }
        }
    }
}
