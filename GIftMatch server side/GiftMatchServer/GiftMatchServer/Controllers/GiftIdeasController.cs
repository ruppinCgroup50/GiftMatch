using GiftMatchServer.BL;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GiftMatchServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftIdeasController : ControllerBase
    {
        // GET: api/<GiftIdeasController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       

        // POST api/<GiftIdeasController>
        [HttpPost]
        public IActionResult Post([FromBody] JsonElement data)
        {
            try
            {


                DBservices dbs = new DBservices();
                int res = dbs.InsertGiftIdea(data);
                if (res > 0)
                {
                    return Ok(res);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    
    }
}
