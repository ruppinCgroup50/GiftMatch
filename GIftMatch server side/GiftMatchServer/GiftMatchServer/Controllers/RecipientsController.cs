using GiftMatchServer.BL;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GiftMatchServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipientController : ControllerBase
    {
        // GET: api/<RecipientController>
        [HttpGet]
        [HttpPost("AddNewRecipient")]
        public IActionResult InsertRecipient([FromBody] Recipient recipient)
        {
            try
            {
                DBservices dbs = new DBservices();
                int res = dbs.InsertRecipient(recipient);
                if (res == 1)
                {
                    return Ok(recipient);
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
