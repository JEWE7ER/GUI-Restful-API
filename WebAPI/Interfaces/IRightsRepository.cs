namespace WebAPI.Models
{
    public interface IRightsRepository
    {
        Task<string> FindOneAsync(int id);
        Task<int> FindOneAsync(string right);
    }
}
