using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GiftMatchServer.BL;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Reflection.Emit;

namespace GiftMatchServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GiftMatchController : ControllerBase
    {
        [HttpPost("SubmitIdea")]
        public async Task<IActionResult> SubmitIdea([FromBody] /*Gpt3Submission submission*/JsonElement data)
        {
            try
            {

                string gName = data.GetProperty("giftName").GetString();
                GiftIdea gift = new GiftIdea();
                gift.GiftName = gName;
                gift.Price = Convert.ToInt32(data.GetProperty("price").GetString());
                gift.Image = data.GetProperty("fileName").GetString();
                gift.Description = data.GetProperty("description").GetString();
                gift.UserName = data.GetProperty("email").GetString();

                Gpt3Submission gpt = new Gpt3Submission();
                gpt.GiftName = gName;

                if (data.TryGetProperty("interests", out JsonElement interests) && interests.ValueKind == JsonValueKind.Array)
                {
                    List<string> interestsList = new List<string>();
                    foreach (JsonElement element in interests.EnumerateArray())
                    {
                        interestsList.Add(element.GetString());
                    }
                    gpt.Interests = interestsList;
                }

                // קריאה לפונקציה במחלקה שבודקת התאמה בין תחומי עניין וביג 5 למתנה
                var (compatibleInterests, compatibleBIG5) = await gpt.CheckInterestsCompatibility();
                var response = new
                {
                    compatibleInterests = compatibleInterests,
                    compatibleBIG5 = compatibleBIG5
                };
                if (response.compatibleInterests.Count == 0 || response.compatibleBIG5.Count == 0)
                    return NotFound("שם המתנה לא מתאים לתחומים שנבחרו");

                DBservices dBservices = new DBservices();


                string InterestsString = "";
                foreach (var item in response.compatibleInterests)
                {
                    InterestsString += item + ",";
                }
                string AttrString = "";
                foreach (var item in response.compatibleBIG5)
                {
                    AttrString += item + ",";
                }
                int res = dBservices.InsertGiftIdea(gift);
                if (res == 0)
                {
                    return NotFound("שגיאה בעת הוספת רעיון");
                }
                int resIntres = dBservices.InsertGiftInterest(gName, InterestsString);
                int resAttr = dBservices.InsertGiftAttr(gName, AttrString);
                if (resIntres > 0 && resAttr > 0)
                    return Ok(res);
                else
                    return NotFound("שגיאה בעת שיוך רעיונות/תכונות למתנה");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }



        }
    }
}
