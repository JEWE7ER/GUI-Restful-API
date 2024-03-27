using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    internal class AuthorizeFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            string? headerName = context.HttpContext.Request.Headers.Authorization;

            if (!string.IsNullOrEmpty(headerName))
            {
                if (headerName.StartsWith("Basic "))
                {
                    try
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUsersRepository>();
                        Tuple<bool, string> result = await IsUserAllowedAsync(headerName, userService);
                        bool isValid = result.Item1;
                        if (!isValid)
                        {
                            context.Result = new UnauthorizedResult();
                        }
                        else
                        {
                            context.RouteData.Values.Add("login", result.Item2);
                        }
                    }
                    catch (Exception)
                    {
                        context.Result = new UnauthorizedResult();
                    }
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private async Task<Tuple<bool, string>> IsUserAllowedAsync(string headerName, IUsersRepository userService)
        {
            string encodedToken = headerName["Basic ".Length..].Trim();
            string headerValue = Encoding.UTF8.GetString(Convert.FromBase64String(encodedToken));
            string[] credentials = headerValue.Split([':'], 2);

            string username = credentials[0];
            string password = credentials[1];

            bool validate = await userService.ValidateAsync(username, password);
            return Tuple.Create(validate, username);
        }
    }

    public class Authorization(MySqlDataSource database): IAuthorization
    {
        public async Task<List<string>> Login(string login)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                                    select r.right from rights as r
                                    join (users as u, users_rights as ur) 
	                                    on (u.login=@login and
                                            ur.users_id = u.id and 
                                            ur.rights_id = r.id);
                                   ";
            command.Parameters.AddWithValue("@login", login);
            var result = await ReadAllAsync(await command.ExecuteReaderAsync());
            return result;
        }

        private static async Task<List<string>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<string>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    posts.Add(reader.GetString(0));
                }
            }
            return posts;
        }
    }
}
