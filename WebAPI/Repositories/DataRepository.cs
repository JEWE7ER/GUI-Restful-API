using MySqlConnector;
using System.Data.Common;

namespace WebAPI.Models
{
    public class DataRepository(MySqlDataSource database) : IDataRepository
    {
        public async Task<Data?> FindOneAsync(int id)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT `id`, `value` FROM `data` WHERE `id` = @id";
            command.Parameters.AddWithValue("@id", id);
            var result = await ReadAllAsync(await command.ExecuteReaderAsync());
            return result[0];
        }

        public async Task<IReadOnlyList<Data>> GetAllAsync()
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT `id`, `value` FROM `data` ORDER BY `id` DESC;";
            return await ReadAllAsync(await command.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM `data`";
            await command.ExecuteNonQueryAsync();
        }

        public async Task InsertAsync(Data Data)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO `Data` (`value`) VALUES (@value);";
            BindParameters(command, Data);
            await command.ExecuteNonQueryAsync();
            Data.id = (int)command.LastInsertedId;
        }

        public async Task UpdateAsync(Data Data)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"UPDATE `data` SET `value` = @value WHERE `id` = @id;";
            BindParameters(command, Data);
            BindId(command, Data);
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(Data Data)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM `data` WHERE `id` = @id;";
            BindId(command, Data);
            await command.ExecuteNonQueryAsync();
        }

        private static async Task<IReadOnlyList<Data>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Data>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Data
                    {
                        id = reader.GetInt32(0),
                        value = reader.GetString(1)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        private static void BindId(MySqlCommand cmd, Data Data) =>
            cmd.Parameters.AddWithValue("@id", Data.id);

        private static void BindParameters(MySqlCommand cmd, Data Data) =>
            cmd.Parameters.AddWithValue("@value", Data.value);
    }
}
