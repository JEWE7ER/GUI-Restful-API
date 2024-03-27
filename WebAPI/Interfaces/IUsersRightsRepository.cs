namespace WebAPI.Models
{
    public interface IUsersRightsRepository
    {
        Task<List<int>> FindRightsUserAsync(int id);
        Task InsertAsync(int users_id, int rights_id);
    }
}
