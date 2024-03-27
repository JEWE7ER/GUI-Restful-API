using MySqlConnector;
using System.Data.Common;

namespace WebAPI.Models
{
    public class UsersRightsRepository(MySqlDataSource database) : IUsersRightsRepository
    {
        public async Task<List<int>> FindRightsUserAsync(int id)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT `users_id`, `rights_id` FROM `users_rights` WHERE `users_id` = @users_id";
            command.Parameters.AddWithValue("@users_id", id);
            var result = await ReadAllAsync(await command.ExecuteReaderAsync());
            List<int> rights_id = [];
            foreach (var item in result)
                rights_id.Add(item.rights_id);
            return rights_id;
        }

        public async Task InsertAsync(int users_id, int rights_id)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO `users_rights` (`users_id`, `rights_id`) VALUES (@users_id, @rights_id);";
            command.Parameters.AddWithValue("@users_id", users_id);
            command.Parameters.AddWithValue("@rights_id", rights_id);
            await command.ExecuteNonQueryAsync();
        }

        private static async Task<IReadOnlyList<UsersRights>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<UsersRights>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new UsersRights
                    {
                        users_id = reader.GetInt32(0),
                        rights_id = reader.GetInt32(1)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
