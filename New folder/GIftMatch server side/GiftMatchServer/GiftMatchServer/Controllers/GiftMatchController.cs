using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Reflection.Emit;
using GiftMatchServer.BL;
using Microsoft.AspNetCore.Mvc;

namespace GiftMatchServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiftMatchController : ControllerBase
    {
        [HttpPost("SubmitIdea")]
        public async Task<IActionResult> SubmitIdea([FromBody] JsonElement data)
        {
            try
            {
                // שליפת נתונים מתוך השדות של הגייסון מהקליינט עבור רעיון חדש
                string gName = data.GetProperty("giftName").GetString();
                GiftIdea gift = new GiftIdea();
                gift.GiftName = gName;
                gift.Price = Convert.ToInt32(data.GetProperty("price").GetString());
                gift.Image = data.GetProperty("fileName").GetString();
                gift.Description = data.GetProperty("description").GetString();
                gift.UserName = data.GetProperty("email").GetString();

                Gpt3Submission gpt = new Gpt3Submission();
                gpt.GiftName = gName;

                // שליפת נתונים מתוך השדות של הגייסון מהקליינט עבור רשימת תחומים 
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

                if (response.compatibleInterests.Count == 0 || response.compatibleBIG5.Count == 0)//אם אחת הרשימות שחזרו מהצ'אט ריקות
                    return NotFound("שם המתנה לא מתאים לתחומים שנבחרו");

                DBservices dBservices = new DBservices();
                //יוצרים מחרוזת מופרדת בפסיקים של התחומי עניין לטובת העלאה בס קיו אל לטובת העלאת בעזרת פונקציית ספליט
                string InterestsString = "";
                foreach (var item in response.compatibleInterests)
                {
                    InterestsString += item + ",";
                }
                //יוצרים מחרוזת מופרדת בפסיקים של התכונות לטובת העלאה בס קיו אל לטובת העלאת בעזרת פונקציית ספליט
                string AttrString = "";
                foreach (var item in response.compatibleBIG5)
                {
                    AttrString += item + ",";
                }

                //מעלים רק את הרעיון החדש
                int res = dBservices.InsertGiftIdea(gift);
                if (res == 0)//אם הרעיון לא עלה. רס מייצג שורת שנוספו לטבלה
                {
                    return NotFound("שגיאה בעת הוספת רעיון");
                }
                //מעלים לטבלת רבים לרבים את תחומי העניין
                int resIntres = dBservices.InsertGiftInterest(gName, InterestsString);
                //מעלים לטבלת רבים לרבים את התכונות
                int resAttr = dBservices.InsertGiftAttr(gName, AttrString);
                if (resIntres > 0 && resAttr > 0)//אם התכונות והתחומי עניין לפחות שורה אח הוכנסה בכל טבלה
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