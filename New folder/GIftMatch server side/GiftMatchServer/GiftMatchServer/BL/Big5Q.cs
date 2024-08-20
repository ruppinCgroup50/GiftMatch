using System.Text.Json;

namespace GiftMatchServer.BL
{
    public class Big5Q
    {
     

        public int Id { get; set; }
        public string Qname { get; set; }
        public int AttId { get; set; }

        public Big5Q()
        {
            
        }
        public Big5Q(int id, string qname, int attId)
        {
            Id = id;
            Qname = qname;
            AttId = attId;
        }
        public List<AttributeValue> Get2BestAtrr(List<int> ansArr)
        {
            List<double> avgArr = new List<double>();

            // לולאה שעושה ממוצע לכל 3 תאים ומכניסה את התוצאה למערך הממוצעים
            for (int i = 0; i < ansArr.Count; i += 3)
            {
                var group = ansArr.Skip(i).Take(3).ToList();
                double sum = group.Sum();
                double average = sum / group.Count;
                avgArr.Add(Math.Round(average, 2));
            }

            // הכנסה לג'ייסון את הממוצע והמספר של התכונה
            var avgJsonArr = avgArr.Select((value, index) => new AttributeValue { Value = value, AttId = index + 1 }).ToList();

            // מיון מערך ממוצעים
            avgJsonArr = avgJsonArr.OrderByDescending(x => x.Value).ToList();
            var maxValues = new List<AttributeValue>();
            if (avgArr.All(x => x == avgArr[0]))//כולם שווים
            {
                var rnd = new Random();
                int randomIndex = rnd.Next(0, ansArr.Count / 3); // בחירת אחד מבין הקבוצות
                double average = Math.Round((double)ansArr.Skip(randomIndex * 3).Take(3).Sum() / 3, 2);
                avgJsonArr.Add(new AttributeValue { Value = average, AttId = randomIndex + 1 });
                maxValues = avgJsonArr.Take(2).ToList();
            }
            else if (avgJsonArr[1].Value == avgJsonArr[2].Value|| avgJsonArr[1].Value == avgJsonArr[3].Value || avgJsonArr[1].Value == avgJsonArr[4].Value)//אם יש שיוויון בין הערך שהני לבאים אחריו
            {
               
                maxValues.Add(avgJsonArr[0]); // הערך הגבוה ביותר

                // בדיקה לשני הערכים הבאים בגבוהות
                for (int i = 1; i < avgJsonArr.Count; i++)
                {
                    if (avgJsonArr[i].Value == avgJsonArr[1].Value)
                    {
                        // יש ערך ששווה לערך השני בגבוהות - בחירה רנדומלית בין הערכים
                        var candidates = new List<AttributeValue> { avgJsonArr[1], avgJsonArr[i] };
                        var rnd = new Random();
                        maxValues.Add(candidates[rnd.Next(2)]); // בחירה רנדומלית בין הערכים השווים
                        break; // יציאה מהלולאה לאחר בחירת ערך
                    }
                    else
                    {
                        maxValues.Add(avgJsonArr[i]); // הערך הבא בגבוהות
                    }

                    if (maxValues.Count == 2)
                    {
                        break; // יציאה מהלולאה לאחר בחירת 2 ערכים
                    }
                }

            }
            else
            {
                maxValues = avgJsonArr.Take(2).ToList();
            }
            return maxValues;
        }
    }
}
