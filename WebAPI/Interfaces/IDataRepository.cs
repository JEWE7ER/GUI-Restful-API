using System.Data.Common;

namespace WebAPI.Models
{
    public interface IDataRepository
    {
        Task<IReadOnlyList<Data>> GetAllAsync();
        Task<Data?> FindOneAsync(int id);
        Task InsertAsync(Data data);
        Task UpdateAsync(Data data);
        Task DeleteAllAsync();
        Task DeleteAsync(Data data);
        
    }
}
