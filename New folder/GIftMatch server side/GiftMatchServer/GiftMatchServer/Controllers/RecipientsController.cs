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
        [HttpPost("insertNewsAssociatedInterest")]
        public IActionResult insertNewsAssociatedInterest([FromBody] AssociatedInterest interest)
        {
            try
            {
                DBservices dbs = new DBservices();
                int res = dbs.insertNewsAssociatedInterest(interest);

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

        [HttpPost("insertNewsAssociatedAttr")]
        public IActionResult insertNewsAssociatedAttr([FromBody] AssociatedAtrr attr)
        {
            try
            {
                DBservices dbs = new DBservices();
                int res = dbs.insertNewsAssociatedAttr(attr);

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
        [HttpGet("getRecipientAssociatedAttr/{phone}")]
        public IActionResult getRecipientAssociatedAttr(string phone)
        {
            try
            {
                DBservices dbs = new DBservices();
                List<AssociatedAtrr> res = dbs.getRecipientAssociatedAttr(phone);

                if (res.Count > 0)
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
        [HttpGet("getRecipientAssociatedInterest/{phone}")]
        public IActionResult getRecipientAssociatedInterest(string phone)
        {
            try
            {
                DBservices dbs = new DBservices();
                List<AssociatedInterest> res = dbs.getRecipientAssociatedInterest(phone);

                if (res.Count > 0)
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
