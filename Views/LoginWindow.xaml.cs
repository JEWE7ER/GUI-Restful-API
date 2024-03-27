using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WebAPI.Models;

namespace GUI
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            
        }
        Users user = new();

        private async void authorize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Uri url = new("http://localhost:5074/login");
                string login = this.login.Text;
                string password = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(this.password.Password)));

                using HttpClient client = new()
                {
                    DefaultRequestVersion = HttpVersion.Version20,
                    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
                };

                //string contentType = "application/json";
                string authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes(login + ":" + password));
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {authHeader}");
                //client.DefaultRequestHeaders.Add("Accept", "application/*+xml;version=5.1");
                //var userAgent = "d-fens HttpClient";
                //client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var finalRequestUri = response.RequestMessage!.RequestUri;
                    if (finalRequestUri! != url)
                    {
                        response = await client.GetAsync(finalRequestUri);
                    }
                }

                var content = await response.Content.ReadAsStringAsync();
                var rights = JsonConvert.DeserializeObject<List<string>>(content);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
