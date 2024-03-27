using GUI.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {   
            var loginView = new LoginWindow();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded && Users.account!=null)
                {
                    var mainView = new MainWindow();
                    mainView.Show();
                    loginView.Close();
                }
            }; 
        }
    }

}
