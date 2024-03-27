namespace WebAPI.Interfaces
{
    public interface IAuthorization 
    { 
        Task<List<string>> Login(string login);
    }
}
