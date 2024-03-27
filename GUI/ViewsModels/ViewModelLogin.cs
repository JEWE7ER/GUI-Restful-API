using System.Net;
using System.Text;
using System.Windows.Input;
using System.Net.Http;
using System.Security.Cryptography;
using Newtonsoft.Json;
using GUI.Models;
using WebAPI.Models;
using System.Net.Http.Json;
using Users = GUI.Models.Users;
using System.Windows.Forms;

namespace GUI.ViewsModels
{
    public class ViewModelLogin : ViewModelBase
    {
        //Fields
        private readonly string declineColor = "#D7596D";
        private readonly string acceptColor = "#07F3C0";
        private string? _color;
        private string? _username;
        private string? _password;
        private string? _errorMessage;
        private bool _isViewVisible = true;
        private bool _isButtonEnable = true;
        private bool _isCheckLoginButton;
        private bool _isCheckRegButton;
        private bool _isRegistered = false;
        private ViewModelBase? _childView; 
        //private readonly ViewModelLoginForContent? _loginView = new(); 
        //private readonly ViewModelRegister? registerView = new(); 
        private string? _caption; 
        //private string? _helpText; 
        //private string? _helpLink;
        //private IconChar _icon; 
        //Properties
        public string Username
        {
            get
            {
                return _username!;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get
            {
                return _password!;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get
            {
                return _errorMessage!;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        public bool IsButtonEnable
        {
            get => _isButtonEnable;
            set
            {
                _isButtonEnable = value;
                OnPropertyChanged(nameof(IsButtonEnable));
            }
        }
        public string Color
        {
            get
            {
                return _color!;
            }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        public ViewModelBase? ChildView 
        {
            get
            {
                return _childView!;
            }
            set
            {
                _childView = value;
                OnPropertyChanged(nameof(ChildView));
            }
        }
        public string Caption 
        {
            get
            {
                return _caption!;
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        public bool IsCheckLoginButton
        {
            get
            {
                return _isCheckLoginButton;
            }
            set
            {
                _isCheckLoginButton = value;
                OnPropertyChanged(nameof(IsCheckLoginButton));
            }
        }

        public bool IsCheckRegButton
        {
            get
            {
                return _isCheckRegButton;
            }
            set
            {
                _isCheckRegButton = value;
                OnPropertyChanged(nameof(IsCheckRegButton));
            }
        }

        //public ViewModelLoginForContent? LoginView
        //{
        //    get => _loginView;
        //}

        //-> Commands
        public ICommand? LoginCommand { get; }
        public ICommand? RegisterCommand { get; }
        public ICommand? ShowLoginViewCommand { get; }
        public ICommand? ShowRegisterViewCommand { get; }
        

        //Constructor
        public ViewModelLogin()
        {
            //Initialize commands
            ShowLoginViewCommand = new ViewModelCommand(ExecuteShowLoginViewCommand);
            ShowRegisterViewCommand = new ViewModelCommand(ExecuteShowRegisterViewCommand);
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);

            //Default view
            ExecuteShowLoginViewCommand(null);
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else
                validData = true;
            return validData;
        }
        private async void ExecuteLoginCommand(object? obj)
        {
            Color = declineColor;
            IsButtonEnable = false;
            ErrorMessage = "";
            Uri url = new("https://localhost:7046/login");
            string password = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(Password)));

            using HttpClient client = new()
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
            };

            string authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes(Username + ":" + password));
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {authHeader}");
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var finalRequestUri = response.RequestMessage!.RequestUri;
                    if (finalRequestUri! != url)
                    {
                        response = await client.GetAsync(finalRequestUri);
                    }
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var encryptedData = Convert.FromBase64String(await response.Content.ReadAsStringAsync());
                    //var content = await response.Content.ReadAsStringAsync();
                    string decryptedData = AesCrypt.Decrypt(encryptedData);
                    var rights = JsonConvert.DeserializeObject<List<string>>(decryptedData)!;
                    //var rights = JsonConvert.DeserializeObject<List<string>>(content)!;
                    Users.SaveInfo(Username, password, rights);
                    Users.IsLoggedIn = true;
                    IsViewVisible = false;
                }
                else
                {
                    ErrorMessage = "* Неверно введён логин или пароль";
                }
            }
            catch
            {
                ErrorMessage = "* Сервер вне доступа";
            }
            finally
            {
                IsButtonEnable = true;
            }

        }
        private bool CanExecuteRegisterCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else
                validData = true;
            return validData;
        }
        private async void ExecuteRegisterCommand(object? obj)
        {
            Color = declineColor;
            IsButtonEnable = false;
            ErrorMessage = "";
            Uri url = new("https://localhost:7046/registration");

            using HttpClient client = new()
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
            };
            string password = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(Password)));
            var user = new UserForRequest(Username, password);
            string dataToSend = JsonConvert.SerializeObject(user);
            byte[] encryptedData = AesCrypt.Encrypt(dataToSend);
            //var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            try
            {
                var content = new ByteArrayContent(encryptedData);
                //HttpResponseMessage response = await client.PostAsync(url, content);
                HttpResponseMessage response = await client.PostAsJsonAsync(url, content);

                //if (response.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    var finalRequestUri = response.RequestMessage!.RequestUri;
                //    if (finalRequestUri! != url)
                //    {
                //        response = await client.GetAsync(finalRequestUri);
                //    }
                //}
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _isRegistered = Convert.ToBoolean(await response.Content.ReadAsStringAsync());
                    if (_isRegistered)
                    {
                        ExecuteShowLoginViewCommand(null);
                    }
                    else
                    {
                        ErrorMessage = "* Такой уже существует";
                    }
                }
                else
                {
                    ErrorMessage = $"* {response.StatusCode}";
                }
            }
            catch
            {
                ErrorMessage = "* Сервер вне доступа";
            }
            finally
            {
                IsButtonEnable = true;
            }

        }
        private void ExecuteShowLoginViewCommand(object? obj)
        {
            if (_isRegistered)
            {
                ErrorMessage = "* Вы успешно зарегистрированы";
                Color = acceptColor;
            }
            else
            {
                ErrorMessage = "";
                Username = "";
            }
            ChildView = new ViewModelLoginForContent();
            Caption = "ВХОД";
            IsCheckLoginButton = true;
        }
        private void ExecuteShowRegisterViewCommand(object obj)
        {
            ErrorMessage = "";
            Username = "";
            ChildView = new ViewModelRegister();
            Caption = "РЕГИСТРАЦИЯ";
            IsCheckRegButton = true;
        }
    }
}
