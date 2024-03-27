using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Windows.Input;
using GUI.Models;
using System.Security.Cryptography;

namespace GUI.ViewsModels
{
    public class ViewModelLoginForContent : ViewModelBase
    {
        //Fields
        //private string? _username;
        //private string? _password;
        //private string? _message = "";
        //private string? _color = "#D7596D";
        //private bool _isViewVisible = true;
        //private bool _isButtonEnable = true;
        ////Properties
        //public string Username
        //{
        //    get
        //    {
        //        return _username!;
        //    }
        //    set
        //    {
        //        _username = value;
        //        OnPropertyChanged(nameof(Username));
        //    }
        //}
        //public string Password
        //{
        //    get
        //    {
        //        return _password!;
        //    }
        //    set
        //    {
        //        _password = value;
        //        OnPropertyChanged(nameof(Password));
        //    }
        //}
        //public string Message
        //{
        //    get
        //    {
        //        return _message!;
        //    }
        //    set
        //    {
        //        _message = value;
        //        OnPropertyChanged(nameof(Message));
        //    }
        //}

        //public string Color
        //{
        //    get
        //    {
        //        return _color!;
        //    }
        //    set
        //    {
        //        _color = value;
        //        OnPropertyChanged(nameof(Color));
        //    }
        //}

        //public new bool IsViewVisible
        //{
        //    get
        //    {
        //        return _isViewVisible;
        //    }
        //    set
        //    {
        //        _isViewVisible = value;
        //        OnPropertyChanged(nameof(IsViewVisible));
        //    }
        //}

        //public bool IsButtonEnable
        //{
        //    get => _isButtonEnable;
        //    set
        //    {
        //        _isButtonEnable = value;
        //        OnPropertyChanged(nameof(IsButtonEnable));
        //    }
        //}

        ////-> Commands
        //public ICommand? LoginCommand { get; }

        ////Constructor
        //public ViewModelLoginForContent()
        //{
        //    //Initialize commands
        //    LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        //}

        //private bool CanExecuteLoginCommand(object obj)
        //{
        //    bool validData;
        //    if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
        //        Password == null || Password.Length < 3)
        //        validData = false;
        //    else
        //        validData = true;
        //    return validData;
        //}
        //private async void ExecuteLoginCommand(object? obj)
        //{
        //    IsButtonEnable = false;
        //    Message = "";
        //    Uri url = new("https://localhost:7046/login");
        //    string password = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(Password)));

        //    using HttpClient client = new()
        //    {
        //        DefaultRequestVersion = HttpVersion.Version20,
        //        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
        //    };

        //    string authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes(Username + ":" + password));
        //    client.DefaultRequestHeaders.Add("Authorization", $"Basic {authHeader}");
        //    try
        //    {
        //        HttpResponseMessage response = await client.GetAsync(url);

        //        if (response.StatusCode == HttpStatusCode.Unauthorized)
        //        {
        //            var finalRequestUri = response.RequestMessage!.RequestUri;
        //            if (finalRequestUri! != url)
        //            {
        //                response = await client.GetAsync(finalRequestUri);
        //            }
        //        }
        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            var content = await response.Content.ReadAsStringAsync();
        //            var rights = JsonConvert.DeserializeObject<List<string>>(content)!;
        //            Users.SaveInfo(Username, password, rights);
        //            Users.IsLoggedIn = true;
        //            IsViewVisible = false;
        //        }
        //        else
        //        {
        //            Message = "* Неверно введён логин или пароль";
        //        }
        //    }
        //    catch
        //    {
        //        Message = "* Сервер вне доступа";
        //    }
        //    finally
        //    {
        //        IsButtonEnable = true;
        //    }

        //}
        
    }
}
