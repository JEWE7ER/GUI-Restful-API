namespace WebAPI.Models
{
    public interface IUsersRepository
    {
        Task<IReadOnlyList<Users>> GetAllAsync();
        Task<Users?> FindOneAsync(string login);
        Task<bool> ValidateAsync(string login, string password);
        Task InsertAsync(Users user);
        Task UpdateAsync(Users user);
        Task DeleteAllAsync();
        Task DeleteAsync(Users user);
    }
}
