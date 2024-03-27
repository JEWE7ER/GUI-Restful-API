using MySqlConnector;
using System.Data.Common;

namespace WebAPI.Models
{
    public class UsersRepository(MySqlDataSource database) : IUsersRepository
    {
        public async Task<Users?> FindOneAsync(string login)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT `id`, `login`, `password` FROM `users` WHERE `login` = @login";
            command.Parameters.AddWithValue("@login", login);
            var result = await ReadAllAsync(await command.ExecuteReaderAsync());
            return result.Count == 0 ? null : result[0];
        }
        
        public async Task<bool> ValidateAsync(string login, string password)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT `id`, `login`, `password` FROM `users` WHERE `login` = @login and `password` = @password";
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@password", password);
            var result = await ReadAllAsync(await command.ExecuteReaderAsync());
            return result.Count == 1;
        }

        public async Task<IReadOnlyList<Users>> GetAllAsync()
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT `id`, `login`, `password` FROM `users` ORDER BY `id` DESC;";
            return await ReadAllAsync(await command.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM `users`";
            await command.ExecuteNonQueryAsync();
        }

        public async Task InsertAsync(Users user)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO `users` (`login`,`password`) VALUES (@login, @password);";
            BindParameters(command, user);
            await command.ExecuteNonQueryAsync();
            user.id = (int)command.LastInsertedId;
        }

        public async Task UpdateAsync(Users user)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"UPDATE `users` SET `password` = @password WHERE `id` = @id;";
            BindParameters(command, user);
            BindId(command, user);
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(Users user)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM `users` WHERE `id` = @id;";
            BindId(command, user);
            await command.ExecuteNonQueryAsync();
        }

        private static async Task<IReadOnlyList<Users>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Users>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Users
                    {
                        id = reader.GetInt32(0),
                        login = reader.GetString(1),
                        password = reader.GetString(2)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        private static void BindId(MySqlCommand cmd, Users user) =>
            cmd.Parameters.AddWithValue("@id", user.id);

        private static void BindParameters(MySqlCommand cmd, Users user)
        {
            cmd.Parameters.AddWithValue("@login", user.login);
            cmd.Parameters.AddWithValue("@password", user.password);
        }
    }
}
