using GiftMatchServer.BL;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


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
                return NotFound("שגיאת שרת- נסו שוב");
            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("PRIMARY"))
                    return BadRequest("אימייל זה כבר נמצא במערכת");
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
                return NotFound("אימייל/סיסמא אינם נכונים אנא נסו שנית");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("CheckIfUserExists/{email}")]
        public IActionResult CheckIfUserExists(string email)
        {
            try
            {
                DBservices dbs = new DBservices();
                int res = dbs.CheckIfUserExists(email);
                if (res == 1)
                {
                    return Ok(res);
                }
                return NotFound("אימייל/סיסמא לא קיימים במערכת");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("updatePassword")]
        public IActionResult updatePassword([FromBody] JsonElement data)
        {
            try
            {
                string email = data.GetProperty("email").GetString();
                string password = data.GetProperty("password").GetString();
                DBservices dbs = new DBservices();
                User res = dbs.updatePassword(email,password);
                if (res != null)
                {
                    return Ok(res);
                }
                return NotFound("אימייל/סיסמא אינם נכונים אנא נסו שנית");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
