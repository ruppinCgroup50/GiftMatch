using GiftMatchServer.BL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Collections.Generic;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<Big5qsController>
        [HttpGet]
        public IActionResult GetQuestion()
        {
            DBservices dbs = new DBservices();
            List<Big5Q> res = dbs.GetQuestion();
            if (res.Count > 0)
                return Ok(res);
            return NotFound();
        }


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
                        int index =(int) res[i].Value - 1;
                        if (index >= 0 && index < Attributes.Count)
                        {
                            resAttributes.Add(Attributes[index]);
                        }
                        else
                        {
                            return BadRequest("Invalid index in res.");
                        }

                        //צריך לקחת מהשרת את רשימת הרעיונות של 2 התכונות הסופיות
                        //שליחה למחלקה לצורך שליחה לצאט
                    }

                    return Ok(res);//עד שיושלם יש החזרה לטובת בדיקות
                }
                return NotFound("ss");
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
                //קבלת המערך מהצד לקוח
                //צריך לקחת מהשרת את רשימת הרעיונות לתחומי העניין
                //שליחה למחלקה לצורך שליחה לצאט
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
