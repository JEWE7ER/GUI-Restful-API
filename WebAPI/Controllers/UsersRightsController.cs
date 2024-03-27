using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/gettest")]
    [ApiController]
    public class UsersRightsController(IUsersRightsRepository repository) : ControllerBase
    {
        public IUsersRightsRepository Repository { get; set; } = repository;

        [HttpGet]
        public async Task<IActionResult> FindRightsUserAsync(int id)
        {
            var result = await Repository.FindRightsUserAsync(id);
            return result == null ? BadRequest() : Ok(result);
        }
    }
}
