namespace GiftMatchServer.BL
{
    public class AttributeMatchingPercentage
    {
        public string Phone { get; set; }
        public int MatchingPercentage { get; set; }
        public int Id { get; set; }


        public AttributeMatchingPercentage()
        {

        }
        public AttributeMatchingPercentage(string phone, int matchingPercentage, int id)
        {
            Phone = phone;
            MatchingPercentage = matchingPercentage;
            Id= id;
        }
    }
}
