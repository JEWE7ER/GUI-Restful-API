using WebAPI.Models;
using WebAPI.Interfaces;
using WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Org.BouncyCastle.Asn1.Cms;
using System.Text;

namespace WebAPI.Controllers
{
    [ApiController]
    public class UsersController(IUsersRepository users_repository, IAuthorization authorization, IUsersRightsRepository usersRightsRepository) : ControllerBase
    {
        public IUsersRepository Repository { get; set; } = users_repository;
        public IAuthorization RepositoryAuthorization { get; set; } = authorization;
        public IUsersRightsRepository URRepository { get; set; } = usersRightsRepository;

        // GET: /read
        [HttpGet("logins", Name = "logins")]
        public async Task<IReadOnlyList<Users>> GetAllAsync()
        {
            return await Repository.GetAllAsync();
        }
        [AuthorizeFilter]
        // GET: /read
        [HttpGet("login", Name = "login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<string>> Login()
        {
            string login = RouteData.Values["login"]!.ToString()!;
            var rights = await RepositoryAuthorization.Login(login);
            string dataToSend = JsonConvert.SerializeObject(rights);
            var encryptedData = AesCrypt.Encrypt(dataToSend);
            return Convert.ToBase64String(encryptedData);
        }
        //public async Task<ActionResult<List<string>>> Login()
        //{
        //    string login = RouteData.Values["login"]!.ToString()!;
        //    return await RepositoryAuthorization.Login(login);
        //}

        //// GET api/<UsersController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST /registration>
        [AllowAnonymous]
        [HttpPost("registration", Name = "registration")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] byte[] encryptedData)
        {
            string decryptedData = AesCrypt.Decrypt(encryptedData);
            var user = JsonConvert.DeserializeObject<Users>(decryptedData);
            if (await Repository.FindOneAsync(user!.login!) == null)
            {
                await Repository.InsertAsync(user);
                await URRepository.InsertAsync(user.id, 1);
                return Ok(true);
            }
            else
            { return Ok(false); }
        }
        //public async Task<IActionResult> Register([FromBody] Users user)
        //{
        //    if (await Repository.FindOneAsync(user.login!) == null)
        //    {
        //        await Repository.InsertAsync(user);
        //        await URRepository.InsertAsync(user.id, 1);
        //        return Ok(true);
        //    }
        //    else
        //    { return Ok(false); }
        //}

        //// PUT api/<UsersController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UsersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
