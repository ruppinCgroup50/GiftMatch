using GiftMatchServer.BL;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GiftMatchServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        [HttpPost("AddNewUser")]
        public IActionResult InsertUser([FromBody] User user)
        {
            try
            {
                DBservices dbs = new DBservices();
                int res = dbs.InsertUser(user);
                if (res == 1)
                {
                    return Ok(user);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Connect")]
        public IActionResult connectUser([FromBody] JsonElement data)
        {
            try
            {
                string email= data.GetProperty("email").GetString();
                string password= data.GetProperty("password").GetString();

                DBservices dbs = new DBservices();
                User res = dbs.connect(email,password);
                if (res!=null)
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
