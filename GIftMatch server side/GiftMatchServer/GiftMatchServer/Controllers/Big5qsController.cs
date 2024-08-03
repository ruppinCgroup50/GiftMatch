using GiftMatchServer.BL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GiftMatchServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Big5qsController : ControllerBase
    {
        // רשימת התכונות
        private static readonly List<string> Attributes = new List<string>
        {
            "Extraversion",
            "Agreeableness",
            "Conscientiousness",
            "Openness",
            "Neuroticism"
        };

        // מילון המכיל את רשימות המתנות לפי תכונות
        private static Dictionary<string, List<string>> giftsByAttributes = new Dictionary<string, List<string>>();

        // משתנה סטטי לשמירת unifiedList
        private static List<string> unifiedList = new List<string>();
        // משתנה סטטי לשמירת מספר הטלפון
        private static string recipientPhoneNumber;


        // POST: api/Big5qsController/RecipientPhone
        [HttpPost("RecipientPhone")]
        public IActionResult PostRecipientPhone([FromBody] JsonElement data)
        {
            try
            {
                if (data.TryGetProperty("phone", out JsonElement phoneElement) && phoneElement.ValueKind == JsonValueKind.String)
                {
                    recipientPhoneNumber = phoneElement.GetString();
                    return Ok("Phone number received and saved successfully.");
                }
                return BadRequest("Invalid data. Phone number not received.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


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
                    // רשימת התגובות מהלקוח
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

                    // מציאת 2 התכונות הבולטות
                    Big5Q b5 = new Big5Q();
                    List<AttributeValue> res = b5.Get2BestAtrr(arr);

                    // יצירת רשימה עם שמות 2 התכונות הבולטות מהביג5
                    List<string> resAttributes = new List<string>();
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

                    // שליפת רשימת המתנות לפי תכונות
                    DBservices dbs = new DBservices();
                    List<GiftList> results = dbs.GetGiftList();
                    if (results.Count > 0)
                    {
                        // מילון חדש לאחסון רשימות המתנות לפי התכונות הבולטות
                        Dictionary<string, List<string>> newGiftsByAttributes = new Dictionary<string, List<string>>();
                        foreach (var attribute in resAttributes)
                        {
                            newGiftsByAttributes[attribute] = new List<string>();
                        }

                        // מילון חדש לאחסון רשימות המתנות
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

                        giftsByAttributes = newGiftsByAttributes;

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

        // POST: api/Big5qsController/InterestsToGpt
        [HttpPost("InterestsToGpt")]
        public async Task<IActionResult> InterestsToGpt([FromBody] JsonElement data)
        {
            try
            {

                    data.TryGetProperty("phone", out JsonElement phoneElement);
                    string phone = phoneElement.GetString();
                if (data.TryGetProperty("selectedInterests", out JsonElement interestsElement) && interestsElement.ValueKind == JsonValueKind.Array)
                {
                    // מילון חדש לאחסון רשימות העניינים
                    Dictionary<string, List<string>> interestLists = new Dictionary<string, List<string>>();

                    // הוספת תחומי העניין מהבקשה למילון
                    foreach (JsonElement interest in interestsElement.EnumerateArray())
                    {
                        if (interest.TryGetProperty("name", out JsonElement nameElement) && nameElement.ValueKind == JsonValueKind.String)
                        {
                            string interestName = nameElement.GetString();
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

                    // שליפת רשימת המתנות לפי תחומי העניין
                    DBservices dbs = new DBservices();
                    List<GiftListInterest> results = dbs.GetGiftListInterest();
                    if (results.Count > 0)
                    {
                        // הכנת רשימות המתנות לפי תחומי העניין
                        foreach (var gift in results)
                        {
                            if (interestLists.ContainsKey(gift.InterestName))
                            {
                                interestLists[gift.InterestName].Add(gift.GiftName);
                            }
                        }
                    }

                    // קריאה לפונקציה שמעבדת את רשימות המתנות
                    Gpt3GiftList gpt3GiftList = new Gpt3GiftList();
                    var (updatedGiftsByAttributes, updatedInterestLists) = await gpt3GiftList.ProcessGiftLists(interestLists, giftsByAttributes).ConfigureAwait(false);

                    // חישוב ציוני המתנות והעניינים
                    Dictionary<string, Dictionary<string, double>> giftsByAttributesScores = CalculateScoresAtt(giftsByAttributes,phone);
                    Dictionary<string, Dictionary<string, double>> interestListsScores = CalculateScoresInter(interestLists, phone);

                    // מיזוג הרשימות ומיון לפי הציונים
                    unifiedList = MergeLists(giftsByAttributesScores, interestListsScores);

                    // החזרת הרשימה המאוחדת
                    return Ok(unifiedList);
                }
                return BadRequest("Invalid data.");
            }
            catch (Exception ex)
            {
                // הדפסת השגיאה במקרה של חריגה
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Big5qsController/unifiedList
        [HttpGet("unifiedList")]
        public IActionResult GetUnifiedList()
        {
            DBservices dbs = new DBservices();
            List<GiftIdea> res = dbs.GetUnifiedList();

            if (res == null || res.Count == 0)
                return NotFound("אין רעיונות מתאימים.");

            // סינון הרשימה כך שתכלול רק את הפריטים שנמצאים ב-unifiedList
            var filteredList = res.Where(g => unifiedList.Contains(g.GiftName)).ToList();

            if (filteredList.Count == 0)
                return NotFound("אין רעיונות מתאימים.");

            // סדר את הרשימה בהתאם לסדר ב-unifiedList
            var orderedList = unifiedList
                              .Where(name => filteredList.Any(g => g.GiftName == name))
                              .SelectMany(name => filteredList.Where(g => g.GiftName == name))
                              .ToList();

            return Ok(orderedList);
        }



        // חישוב ציוני המתנות לתכונות
        private Dictionary<string, Dictionary<string, double>> CalculateScoresAtt(Dictionary<string, List<string>> dictionary,string phone)
        {
            //לקיחת אחוז מידה הכרות מהדאטה בייס
            DBservices dbs = new DBservices();
            List<RecipientRelationshipScore> res = dbs.GetRecipientRelationshipScore(phone);
            int relationshipScore = res.First().RelationshipScore;

            //לקיחת אחוז התאמה מהדאטה בייס
            List<AttributeMatchingPercentage> matchingPercentages = dbs.GetAttributeMatchingPercentage(phone);

            Dictionary<string, Dictionary<string, double>> scores = new Dictionary<string, Dictionary<string, double>>();

            if (relationshipScore > 5)
            {
                double dictionaryScore = (relationshipScore - 5) / 10.0;

                //לקחת אחוזי ציון מהדאטה בייס לכל תכונה 
                //double listScore = dictionaryScore / dictionary.Count;  //במקום זה - להשתמש באחוז ציון מהדאטה בייס

                foreach (var list in dictionary)
                {
                    Dictionary<string, double> itemScores = new Dictionary<string, double>();
                    for (int i = 0; i < list.Value.Count; i++)
                    {
                        double itemScore = 100.0 - (i * (100.0 / list.Value.Count));
                        itemScores[list.Value[i]] = itemScore;
                    }
                    scores[list.Key] = itemScores;
                }

                foreach (var list in scores)
                {
                    // חיפוש אחוז התאמה לפי ID התכונה
                    int attributeId = Attributes.IndexOf(list.Key) + 1;
                    double listScore = matchingPercentages
                        .Where(m => m.Id == attributeId)
                        .Select(m => m.MatchingPercentage)
                        .FirstOrDefault();

                    foreach (var item in list.Value)
                    {
                        double itemFinalScore = item.Value * listScore / 100;
                        scores[list.Key][item.Key] = itemFinalScore;
                    }
                }
            }
            return scores;
        }
        // חישוב ציוני המתנות לתחומי עניין
        private Dictionary<string, Dictionary<string, double>> CalculateScoresInter(Dictionary<string, List<string>> dictionary,string phone)
        {
            //לקיחת אחוז מידה הכרות מהדאטה בייס
            DBservices dbs = new DBservices();
            List<RecipientRelationshipScore> res = dbs.GetRecipientRelationshipScore(phone);
            int relationshipScore = res.First().RelationshipScore;

            Dictionary<string, Dictionary<string, double>> scores = new Dictionary<string, Dictionary<string, double>>();
            if (relationshipScore <= 5) { 
                double dictionaryScore = 100.0;

            // Compute scores for each list (similar to how item scores are computed)
            Dictionary<string, double> listScores = new Dictionary<string, double>();
            var lists = dictionary.Keys.ToList();
            for (int i = 0; i < lists.Count; i++)
            {
                double listScore = 100.0 - (i * (100.0 / lists.Count));
                listScores[lists[i]] = listScore;
            }

            foreach (var list in dictionary)
            {
                Dictionary<string, double> itemScores = new Dictionary<string, double>();
                for (int i = 0; i < list.Value.Count; i++)
                {
                    double itemScore = 100.0 - (i * (100.0 / list.Value.Count));
                    itemScores[list.Value[i]] = itemScore;
                }
                scores[list.Key] = itemScores;
            }

            foreach (var list in scores)
            {
                double listFinalScore = listScores[list.Key] * dictionaryScore / 100;
                foreach (var item in list.Value)
                {
                    double itemFinalScore = item.Value * listFinalScore / 100;
                    scores[list.Key][item.Key] = itemFinalScore;
                }
            }
            }
            else {
                double dictionaryScore = ( 10 - (relationshipScore - 5) ) / 10.0;

                // Compute scores for each list (similar to how item scores are computed)
                Dictionary<string, double> listScores = new Dictionary<string, double>();
                var lists = dictionary.Keys.ToList();
                for (int i = 0; i < lists.Count; i++)
                {
                    double listScore = 100.0 - (i * (100.0 / lists.Count));
                    listScores[lists[i]] = listScore;
                }

                foreach (var list in dictionary)
                {
                    Dictionary<string, double> itemScores = new Dictionary<string, double>();
                    for (int i = 0; i < list.Value.Count; i++)
                    {
                        double itemScore = 100.0 - (i * (100.0 / list.Value.Count));
                        itemScores[list.Value[i]] = itemScore;
                    }
                    scores[list.Key] = itemScores;
                }

                foreach (var list in scores)
                {
                    double listFinalScore = listScores[list.Key] * dictionaryScore / 100;
                    foreach (var item in list.Value)
                    {
                        double itemFinalScore = item.Value * listFinalScore / 100;
                        scores[list.Key][item.Key] = itemFinalScore;
                    }
                }
            }
            return scores;
        }
           






        // מיזוג הרשימות
        private List<string> MergeLists(Dictionary<string, Dictionary<string, double>> giftsByAttributesScores, Dictionary<string, Dictionary<string, double>> interestListsScores)
        {
            Dictionary<string, double> mergedScores = new Dictionary<string, double>();

            // איחוד הרשימות וחיבור הציונים
            void Merge(Dictionary<string, Dictionary<string, double>> source)
            {
                foreach (var list in source)
                {
                    foreach (var item in list.Value)
                    {
                        if (mergedScores.ContainsKey(item.Key))
                        {
                            mergedScores[item.Key] += item.Value;
                        }
                        else
                        {
                            mergedScores[item.Key] = item.Value;
                        }
                    }
                }
            }

            Merge(giftsByAttributesScores);
            Merge(interestListsScores);

            // מיון הרשימות לפי הציונים
            var sortedList = mergedScores.OrderByDescending(x => x.Value).ThenBy(x => Guid.NewGuid()).Select(x => x.Key).ToList();

            return sortedList;
        }
    }
}
