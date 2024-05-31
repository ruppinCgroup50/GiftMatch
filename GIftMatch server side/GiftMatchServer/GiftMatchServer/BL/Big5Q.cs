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

            // מקבלים את 2 ממוצעים הגבוהים ביותר
            var maxValues = avgJsonArr.Take(2).ToList();
            return maxValues;
        }
    }
}
