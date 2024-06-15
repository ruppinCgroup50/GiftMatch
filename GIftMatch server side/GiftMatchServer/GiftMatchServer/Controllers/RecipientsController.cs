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

        [HttpGet("GetRecipient/{email}")]
        public IActionResult GetRecipient(string email)
        {
            DBservices dbs = new DBservices();
            List<Recipient> res = dbs.GetRecipient(email);
            if (res.Count > 0)
                return Ok(res);
            return NotFound();
        }    
        [HttpGet("CheckPhoneNumber/{phone}")]
        public IActionResult CheckPhoneNumber(string phone)
        {
            DBservices dbs = new DBservices();
            int res = dbs.CheckPhoneNumber(phone);
                return Ok(res);

        }

    }
}
