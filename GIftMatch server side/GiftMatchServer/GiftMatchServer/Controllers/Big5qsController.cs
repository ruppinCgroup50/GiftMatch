using GiftMatchServer.BL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GiftMatchServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Big5qsController : ControllerBase
    {
        // רשימת מאפיינים
        private static readonly List<string> Attributes = new List<string>
        {
            "Extraversion",
            "Agreeableness",
            "Conscientiousness",
            "Openness",
            "Neuroticism"
        };

        // משתנה סטטי לאחסון הנתונים של giftsByAttributes
        private static Dictionary<string, List<string>> giftsByAttributes = new Dictionary<string, List<string>>();

        // GET: api/Big5qsController/question
        [HttpGet("question")]
        public IActionResult GetQuestion()
        {
            DBservices dbs = new DBservices();
            List<Big5Q> res = dbs.GetQuestion();
            if (res.Count > 0)
                return Ok(res);
            return NotFound();
        }

        // POST: api/Big5qsController/allAnswers
        [HttpPost("allAnswers")]
        public IActionResult allAnswers([FromBody] JsonElement data)
        {
            try
            {
                if (data.TryGetProperty("ansArr", out JsonElement arrElement) && arrElement.ValueKind == JsonValueKind.Array)
                {
                    List<int> arr = new List<int>();
                    foreach (JsonElement element in arrElement.EnumerateArray())
                    {
                        if (element.ValueKind == JsonValueKind.String && int.TryParse(element.GetString(), out int number))
                        {
                            arr.Add(number);
                        }
                        else
                        {
                            return BadRequest("Invalid data in ansArr.");
                        }
                    }
                    Big5Q b5 = new Big5Q();
                    List<AttributeValue> res = b5.Get2BestAtrr(arr);

                    // קוד להחלפת הערכים ברשימת res במשתנים מרשימת Attributes
                    List<string> resAttributes = new List<string>(); //רשימה עם 2 שמות התכונות מהביג5 שהכי מתאימות
                    for (int i = 0; i < res.Count; i++)
                    {
                        int index = (int)res[i].AttId - 1;
                        if (index >= 0 && index < Attributes.Count)
                        {
                            resAttributes.Add(Attributes[index]);
                        }
                        else
                        {
                            return BadRequest("Invalid index in res.");
                        }
                    }

                    // שליפת רשימת מתנות על פי תכונות-קבלת 2 רשימות מהשרת
                    // במקום ליצור פעולת GET נפרדת.
                    DBservices dbs = new DBservices();
                    List<GiftList> results = dbs.GetGiftList();
                    if (results.Count > 0)
                    {
                        // יצירת מילון לאחסון הרשימות עבור 2 המאפיינים
                        Dictionary<string, List<string>> newGiftsByAttributes = new Dictionary<string, List<string>>();
                        foreach (var attribute in resAttributes)
                        {
                            newGiftsByAttributes[attribute] = new List<string>();
                        }

                        // מעבר על התוצאות מה-DB והכנסתן לרשימות המתאימות
                        foreach (var gift in results)
                        {
                            int attrIndex = gift.AttrId - 1;
                            if (attrIndex >= 0 && attrIndex < Attributes.Count)
                            {
                                string attributeName = Attributes[attrIndex];
                                if (newGiftsByAttributes.ContainsKey(attributeName))
                                {
                                    newGiftsByAttributes[attributeName].Add(gift.GiftName);
                                }
                            }
                        }

                        // עדכון המשתנה הסטטי
                        giftsByAttributes = newGiftsByAttributes;

                        //קיבלנו 2 רשימות- כל רשימה בשם של התכונה הדומיננטית. בכל רשימה כל רשימת המתנות השייכת לאותה תכונה. 
                        //לשלוח לצ'אט את 2 הרשימות
                        return Ok(giftsByAttributes);
                    }
                    return NotFound("No gifts found.");
                }
                return BadRequest("Invalid data.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("InterestsToGpt")]
        public IActionResult InterestsToGpt([FromBody] JsonElement data)
        {
            try
            {
                if (data.TryGetProperty("selectedInterests", out JsonElement interestsElement) && interestsElement.ValueKind == JsonValueKind.Array)
                {
                    // יצירת מילון לאחסון הרשימות
                    Dictionary<string, List<string>> interestLists = new Dictionary<string, List<string>>();

                    // מעבר על המערך שנשלח מהצד לקוח
                    foreach (JsonElement interest in interestsElement.EnumerateArray())
                    {
                        if (interest.TryGetProperty("name", out JsonElement nameElement) && nameElement.ValueKind == JsonValueKind.String)
                        {
                            string interestName = nameElement.GetString();

                            // יצירת רשימה חדשה לפי שם תחום העניין
                            if (!interestLists.ContainsKey(interestName))
                            {
                                interestLists[interestName] = new List<string>();
                            }
                        }
                        else
                        {
                            return BadRequest("Invalid data in selectedInterests.");
                        }
                    }

                    // שליפת רשימת המתנות על פי תחומי עניין
                    DBservices dbs = new DBservices();
                    List<GiftListInterest> results = dbs.GetGiftListInterest();
                    if (results.Count > 0)
                    {
                        // מעבר על התוצאות מה-DB והכנסה לרשימות המתאימות
                        foreach (var gift in results)
                        {
                            if (interestLists.ContainsKey(gift.InterestName))
                            {
                                interestLists[gift.InterestName].Add(gift.GiftName);
                            }
                        }
                    }

                    // קריאה למחלקה Gpt3GiftList עם interestLists ו-giftsByAttributes
                    Gpt3GiftList gpt3GiftList = new Gpt3GiftList();
                    gpt3GiftList.ProcessGiftLists(interestLists, giftsByAttributes); 

                    // החזרת הרשימות שנוצרו
                    return Ok(interestLists);
                }
                return BadRequest("Invalid data.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
