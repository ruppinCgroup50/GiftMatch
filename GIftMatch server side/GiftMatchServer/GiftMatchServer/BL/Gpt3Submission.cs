using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GiftMatchServer.BL
{
    public class Gpt3Submission
    {
        public string GiftName { get; set; }
        public List<string> Interests { get; set; }

        private readonly List<string> BIG5 = new List<string>
        {
            "Extraversion",
            "Agreeableness",
            "Conscientiousness",
            "Neuroticism",
            "Openness"
        };


        // פונקציה שבודקת את התאמה של תחומי העניין למתנה
        public async Task<(List<string> compatibleInterests, List<string> compatibleBIG5)> CheckInterestsCompatibility()
        {
            List<string> compatibleInterests = new List<string>();
            List<string> compatibleBIG5 = new List<string>();

            try
            {
                string apiKey = "sk-proj-or3AjcOEEZQFcx8ej0oGT3BlbkFJO393RyeZ7MFYt70wQwQi"; // מפתח API לשירות OpenAI

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey); // הוספת מפתח API לכותרות הבקשה

                    foreach (string interest in Interests) // לולאה שתרוץ על כל תחומי העניין
                    {
                        string prompt = $"האם מתאים לקנות למישהו מתנה של {GiftName} כשתחום העניין המרכזי שלו הוא {interest}? תענה בבקשה רק בכן או לא";
                        var requestData = new
                        {
                            model = "gpt-3.5-turbo", // המודל שבו משתמשים
                            messages = new[]
                            {
                                new { role = "user", content = prompt }
                            },
                            max_tokens = 10 //הגבלת מספר הטוקנים שהתשובה של המודל יכולה להכיל
                        };

                        var content = JsonContent.Create(requestData); // יצירת תוכן JSON לבקשה
                        var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content); // שליחת הבקשה ל-OpenAI

                        if (response.IsSuccessStatusCode) // בדיקה אם הבקשה הצליחה
                        {
                            string responseBody = await response.Content.ReadAsStringAsync(); // קריאת תוכן התשובה
                            var jsonResponse = JObject.Parse(responseBody); // המרה ל-JSON
                            string answer = jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString().Trim(); // חילוץ התשובה

                            if (answer == "כן")
                            {
                                compatibleInterests.Add(interest);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode}");
                            string errorContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Error content: {errorContent}");
                        }
                    }

                    foreach (string trait in BIG5) // לולאה שתרוץ על כל תכונות האישיות במודל BIG5
                    {
                        string prompt = $" האם מתאים לתת למישהו מתנה של {GiftName} כאשר לפי מודל הBIG5 תחום האישיות שמתאים לו הוא {trait}? תענה בבקשה רק בלא או כן";
                        var requestData = new
                        {
                            model = "gpt-3.5-turbo", // המודל שבו משתמשים
                            messages = new[]
                            {
                                new { role = "user", content = prompt }
                            },
                            max_tokens = 10 //הגבלת מספר הטוקנים שהתשובה של המודל יכולה להכיל
                        };

                        var content = JsonContent.Create(requestData); // יצירת תוכן JSON לבקשה
                        var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content); // שליחת הבקשה ל-OpenAI

                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync(); // קריאת תוכן התשובה
                            var jsonResponse = JObject.Parse(responseBody); // המרה ל-JSON
                            string answer = jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString().Trim(); // חילוץ התשובה

                            if (answer == "כן")
                            {
                                compatibleBIG5.Add(trait);
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
            }
            // הדפסת הודעת שגיאה במקרה של חריגה
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return (compatibleInterests, compatibleBIG5);
        }
    }
}