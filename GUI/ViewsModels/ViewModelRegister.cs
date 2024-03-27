using GUI.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Windows.Input;

namespace GUI.ViewsModels
{
    class ViewModelRegister : ViewModelBase
    {
        ////Fields
        //private string? _username;
        //private string? _password;
        //private string? _errorMessage;
        //private bool _isButtonEnable = true;
        //private bool _isRegistered = false;
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
        //public string ErrorMessage
        //{
        //    get
        //    {
        //        return _errorMessage!;
        //    }
        //    set
        //    {
        //        _errorMessage = value;
        //        OnPropertyChanged(nameof(ErrorMessage));
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

        //public bool IsRegistered
        //{
        //    get => _isRegistered;
        //    set
        //    {
        //        _isRegistered = value;
        //        OnPropertyChanged(nameof(IsRegistered));
        //    }
        //}

        ////-> Commands
        //public ICommand? RegisterCommand { get; }

        ////Constructor
        //public ViewModelRegister()
        //{
        //    //Initialize commands
        //    RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);
        //}
        //private bool CanExecuteRegisterCommand(object obj)
        //{
        //    bool validData;
        //    if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
        //        Password == null || Password.Length < 3)
        //        validData = false;
        //    else
        //        validData = true;
        //    return validData;
        //}
        //private async void ExecuteRegisterCommand(object? obj)
        //{
        //    IsButtonEnable = false;
        //    ErrorMessage = "";
        //    Uri url = new("https://localhost:7046/registration");

        //    using HttpClient client = new()
        //    {
        //        DefaultRequestVersion = HttpVersion.Version20,
        //        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
        //    };
        //    var user = new UserForRequest(Username, Password);
        //    var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
        //    try
        //    {
        //        HttpResponseMessage response = await client.PostAsync(url, content);

        //        //if (response.StatusCode == HttpStatusCode.Unauthorized)
        //        //{
        //        //    var finalRequestUri = response.RequestMessage!.RequestUri;
        //        //    if (finalRequestUri! != url)
        //        //    {
        //        //        response = await client.GetAsync(finalRequestUri);
        //        //    }
        //        //}
        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            IsRegistered = true;
        //        }
        //        else
        //        {
        //            ErrorMessage = "* Неверно введён логин или пароль";
        //        }

        //        IsButtonEnable = true;
        //    }
        //    catch
        //    {
        //        ErrorMessage = "* Сервер вне доступа";
        //    }
        //    finally
        //    {
        //        IsButtonEnable = true;
        //    }

        //}
    }
}
