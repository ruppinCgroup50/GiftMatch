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
        private readonly string _apiKey = "sk-proj-or3AjcOEEZQFcx8ej0oGT3BlbkFJO393RyeZ7MFYt70wQwQi";

        public async Task ProcessGiftLists(Dictionary<string, List<string>> interestLists, Dictionary<string, List<string>> giftsByAttributes)
        {
            foreach (var attribute in giftsByAttributes)
            {
                var sortedGifts = await GetSortedGiftsattribute(attribute.Key, attribute.Value);
                if (sortedGifts != null)
                {
                    giftsByAttributes[attribute.Key] = sortedGifts;
                }
            }

            foreach (var interest in interestLists)
            {
                var sortedGifts = await GetSortedGiftsinterest(interest.Key, interest.Value);
                if (sortedGifts != null)
                {
                    interestLists[interest.Key] = sortedGifts;
                }
            }
        }

        private async Task<List<string>> GetSortedGiftsattribute(string listName, List<string> gifts)
        {
            try
            {
                string prompt = $"תסדר לי את רשימת המתנות הבאה: [{string.Join(", ", gifts)}] מהמתנה שהכי מתאימה לתכונה {listName} ועד למתנה שהכי פחות מתאימה. תענה רק בסדר המתנות, בלי מלל נוסף ובלי מספור של מיקומי המתנות.";

                var requestData = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 3000
                };

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                    var content = JsonContent.Create(requestData);
                    var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JObject.Parse(responseBody);
                        string sortedGiftsText = jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString().Trim();

                        if (!string.IsNullOrEmpty(sortedGiftsText))
                        {
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

        private async Task<List<string>> GetSortedGiftsinterest(string listName, List<string> gifts)
        {
            try
            {
                string prompt = $"תסדר לי את רשימת המתנות הבאה: [{string.Join(", ", gifts)}] מהמתנה שהכי מתאימה לתחום עניין {listName}  ועד למתנה שהכי פחות מתאימה. תענה רק בסדר המתנות, בלי מלל נוסף ובלי מספור של מיקומי המתנות.";

                var requestData = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 3000
                };

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                    var content = JsonContent.Create(requestData);
                    var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

                    if (response.IsSuccessStatusCode)
                    {
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
