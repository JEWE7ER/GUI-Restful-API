using MySqlConnector;
using System.Data.Common;

namespace WebAPI.Models
{
    public class RightsRepository(MySqlDataSource database) : IRightsRepository
    {
        public async Task<string> FindOneAsync(int id)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT `id`, `right` FROM `rights` WHERE `id` = @id";
            command.Parameters.AddWithValue("@id", id);
            var result = await ReadAllAsync(await command.ExecuteReaderAsync());
#pragma warning disable CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
            return result.Count == 0 ? "non rights" : result[0].right;
#pragma warning restore CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
        }

        public async Task<int> FindOneAsync(string right)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT `id`, `right` FROM `rights` WHERE `right` = @right";
            command.Parameters.AddWithValue("@right", right);
            var result = await ReadAllAsync(await command.ExecuteReaderAsync());
            return result.Count == 0 ? -1 : result[0].id;
        }

        private static async Task<IReadOnlyList<Rights>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Rights>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Rights
                    {
                        id = reader.GetInt32(0),
                        right = reader.GetString(1)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
