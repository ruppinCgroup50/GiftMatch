using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GiftMatchServer.BL
{
    public class Gpt3GiftList
    {
        // נכון להוסיף את מפתח ה-API שלך כאן
        private readonly string _apiKey = "sk-proj-or3AjcOEEZQFcx8ej0oGT3BlbkFJO393RyeZ7MFYt70wQwQi";

        public async Task<(Dictionary<string, List<string>>, Dictionary<string, List<string>>)> ProcessGiftLists(Dictionary<string, List<string>> interestLists, Dictionary<string, List<string>> giftsByAttributes)
        {
            // עבור כל תכונה ברשימת המתנות לפי תכונה, קרא לפונקציה שמסדרת את רשימת המתנות
            foreach (var attribute in giftsByAttributes)
            {
                var sortedGifts = await GetSortedGifts(attribute.Key, attribute.Value, attributeName: true);
                if (sortedGifts != null)
                {
                    giftsByAttributes[attribute.Key] = sortedGifts;
                }
            }

            // עבור כל תחום עניין ברשימת התחומים לפי תחום עניין, קרא לפונקציה שמסדרת את רשימת המתנות
            foreach (var interest in interestLists)
            {
                var sortedGifts = await GetSortedGifts(interest.Key, interest.Value, attributeName: false);
                if (sortedGifts != null)
                {
                    interestLists[interest.Key] = sortedGifts;
                }
            }

            return (interestLists, giftsByAttributes);
        }

        private async Task<List<string>> GetSortedGifts(string listName, List<string> gifts, bool attributeName)
        {
            try
            {
                // הבניית ההודעה לשליחה ל-API של OpenAI
                string prompt = attributeName
                    ? $"תסדר לי את רשימת המתנות הבאה: [{string.Join(", ", gifts)}] מהמתנה שהכי מתאימה לתכונה {listName} ועד למתנה שהכי פחות מתאימה. תענה רק בסדר המתנות, בלי מלל נוסף ובלי מספור של מיקומי המתנות."
                    : $"תסדר לי את רשימת המתנות הבאה: [{string.Join(", ", gifts)}] מהמתנה שהכי מתאימה לתחום עניין {listName}  ועד למתנה שהכי פחות מתאימה. תענה רק בסדר המתנות, בלי מלל נוסף ובלי מספור של מיקומי המתנות.";

                // המידע לשליחה ל-API
                var requestData = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 3000
                };

                // שליחת הבקשה ל-API של OpenAI
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                    var content = JsonContent.Create(requestData);
                    var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // קריאת התשובה וטיפול בה
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JObject.Parse(responseBody);
                        string sortedGiftsText = jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString().Trim();

                        if (!string.IsNullOrEmpty(sortedGiftsText))
                        {
                            sortedGiftsText = sortedGiftsText.Trim('[', ']');
                            var sortedGifts = sortedGiftsText.Split(new[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            return new List<string>(sortedGifts);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        string errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error content: {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return null;
        }
    }
}
