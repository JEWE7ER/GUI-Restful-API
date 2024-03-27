using GUI.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Windows;
using WebAPI.Models;
using Users = GUI.Models.Users;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Azure;
using System.Net.Http.Json;
using System.Security.Policy;

namespace GUI.ViewsModels
{
    public class ViewModelMain : ViewModelBase
    {
        private readonly string _read = "https://localhost:7046/data/read";
        private readonly string _write = "https://localhost:7046/data/write";
        private UserAccount? _currentUser;

        private ObservableCollection<Data>? _itemsSource;
        private string? _value;

        public UserAccount CurrentUser
        {
            get { return _currentUser!; }
            set 
            { 
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public ObservableCollection<Data> Items
        {
            get { return _itemsSource!; }
            set 
            {
                _itemsSource = value; 
                OnPropertyChanged(nameof(Items));
            }
        }

        public string Value
        {
            get { return _value!; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public ICommand? AddData { get; }

        public ViewModelMain()
        {
            LoadUserData();
            LoadDataGrid();
            AddData = new ViewModelCommand(ExecuteAddDataCommand, CanExecuteAddDataCommand);
        }

        private bool CanExecuteAddDataCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Value) || Value.Length < 1)
                validData = false;
            else
                validData = true;
            return validData;
        }

        private async void ExecuteAddDataCommand(object obj)
        {

            var response = await RequestToService("POST");
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("data Added");
                LoadDataGrid();
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private async void LoadDataGrid()
        {
            try
            {
                var response = await RequestToService("GET");
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    var encryptedData = Convert.FromBase64String(await response.Content.ReadAsStringAsync());
                    string decryptedData = AesCrypt.Decrypt(encryptedData);
                    List<Data>? _data = JsonConvert.DeserializeObject<List<Data>?>(decryptedData);

                   // var json = await response.Content.ReadAsStringAsync();

                    if (_data != null)
                    {
                        Items = new ObservableCollection<Data>(_data.Cast<Data>());
                    }
                }
                else
                {
                    MessageBox.Show($"Server error code {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Server error code {ex}");
            }
        }

        private void LoadUserData()
        {
            
            if (Users.IsLoggedIn) 
            {
                CurrentUser = Users.account!;
            }
            else
            {
                MessageBox.Show("401");
            }
            
        }

        private async Task<HttpResponseMessage> GetRequest(HttpClient client)
        {
            Uri url = new(_read);
            HttpResponseMessage response = new();
            try
            {
                response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var finalRequestUri = response.RequestMessage!.RequestUri;
                    if (finalRequestUri! != url)
                    {
                        return response = await client.GetAsync(finalRequestUri);
                    }
                }
                return response;
            }
            catch
            {
                MessageBox.Show("Сервер вне доступа");
                return response;
            }
        }
        private async Task<HttpResponseMessage> PostRequest(HttpClient client)
        {
            Data data = new()
            {
                value = Value
            };
            string dataToSend = JsonConvert.SerializeObject(data);
            byte[] encryptedData = AesCrypt.Encrypt(dataToSend);

            Uri url = new(_write);
            HttpResponseMessage response = new();
            try
            {
                var content = new ByteArrayContent(encryptedData);
                response = await client.PostAsJsonAsync(url, encryptedData);
                //response = await client.PostAsJsonAsync(_write, data);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var finalRequestUri = response.RequestMessage!.RequestUri;
                    if (finalRequestUri! != url)
                    {
                        return response = await client.PostAsJsonAsync(finalRequestUri, data);
                    }
                }
                return response;
            }
            catch
            {
                MessageBox.Show("Сервер вне доступа");
                return response;
            }
        }
        private async Task<HttpResponseMessage> RequestToService(string method)
        {
            

            using HttpClient client = new()
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
            };
            string authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes(CurrentUser.Username + ":" + CurrentUser.Password));
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {authHeader}");
            if (method == "GET")
            {
                return await GetRequest(client);
            }
            else 
            {
                return await PostRequest(client);
            }
            
            
        }

    }
}
