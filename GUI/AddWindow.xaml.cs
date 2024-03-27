using WebAPI.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
        }

        internal HttpResponseMessage? response;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Data data = new()
            {
                value = dataValue.Text
            };
            using HttpClient client = new();
            response = await client.PostAsJsonAsync("http://localhost:5074/data/write", data);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("data Added");
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
            Close();
        }
    }
}
