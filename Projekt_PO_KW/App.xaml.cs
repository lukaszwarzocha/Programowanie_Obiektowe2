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
            new Views.LoginWindow().Show();
        }
    }
}
