namespace GiftMatchServer.BL
{
    public class AssociatedAtrr
    {
        public int Id { get; set; }
        public string GiftName { get; set; }
        public double MatchingPercentage { get; set; }

        public AssociatedAtrr(int id, string giftName, double matchingPercentage)
        {
            Id = id;
            GiftName = giftName;
            MatchingPercentage = matchingPercentage;
        }
    }

}
