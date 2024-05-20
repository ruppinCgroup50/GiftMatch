using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GiftMatchServer.BL;

namespace GiftMatchServer.Controllers
{
    [ApiController] 
    [Route("[controller]")] 
    public class GiftMatchController : ControllerBase
    {
        [HttpPost("SubmitIdea")] 
        public async Task<IActionResult> SubmitIdea([FromBody] Gpt3Submission submission) 
        {
            if (submission == null) // בדיקה האם הנתונים שנשלחו ריקים או לא חוקיים
            {
                return BadRequest("Invalid submission data."); 
            }
            // קריאה לפונקציה במחלקה שבודקת התאמה בין תחומי עניין וביג 5 למתנה
            var (compatibleInterests, compatibleBIG5) = await submission.CheckInterestsCompatibility(); 
            var response = new 
            {
                compatibleInterests = compatibleInterests, 
                compatibleBIG5 = compatibleBIG5 
            };

            return Ok(response); // מחזיר תשובה של 200 OK עם האובייקט שנוצר כתשובה
        }
    }
}
