using WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Filters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Controllers
{
    [AuthorizeFilter]
    [Route("[controller]/")]
    [ApiController]
    public class DataController(IDataRepository repository) : ControllerBase
    {
        public IDataRepository Repository { get; set; } = repository;

        [HttpGet("read", Name = "getAll")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<string>> GetAllAsync()
        {
            IReadOnlyList<Data> data = await Repository.GetAllAsync();
            string dataToSend = JsonConvert.SerializeObject(data);
            var encryptedData = AesCrypt.Encrypt(dataToSend);
            return Convert.ToBase64String(encryptedData);
        }
        //public async Task<IReadOnlyList<Data>> GetAllAsync()
        //{
        //    return await Repository.GetAllAsync();
        //}

        [HttpGet("read/{id}", Name = "getById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Data>> FindOneAsync(int id)
        {
            var result = await Repository.FindOneAsync(id);
            return result == null ? BadRequest() : result;
        }

        [HttpPost("write", Name = "post")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Insert([FromBody] byte[] encryptedData)
        {
            string decryptedData = AesCrypt.Decrypt(encryptedData);
            var data = JsonConvert.DeserializeObject<Data>(decryptedData);
            await Repository.InsertAsync(data!);
            return Ok();
        }
        //public async Task<ActionResult<Data>> Insert([FromBody] Data data)
        //{
        //    await Repository.InsertAsync(data);
        //    return data;
        //}

        [HttpPut("write/{id}", Name = "update")]
        [ProducesResponseType<Data>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] Data data)
        {
            var result = await Repository.FindOneAsync(id);
            if (result is null)
                return NotFound();
            result.value = data.value;
            await Repository.UpdateAsync(result);
            return Ok(result);
        }

        [HttpDelete("delete/{id}", Name = "deleteById")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Repository.FindOneAsync(id);
            if (result is null)
                return NotFound();
            await Repository.DeleteAsync(result);
            return Ok(result);
        }

        [HttpDelete("delete", Name = "delete")]
        public async Task<IActionResult> DeleteAll()
        {
            await Repository.DeleteAllAsync();
            return Ok();
        }
    }
}
