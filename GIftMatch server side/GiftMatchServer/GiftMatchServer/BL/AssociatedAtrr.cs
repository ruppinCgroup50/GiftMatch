namespace GiftMatchServer.BL
{
    public class AssociatedAtrr
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public double MatchingPercentage { get; set; }
        public AssociatedAtrr()
        {
           
        }
        public AssociatedAtrr(int id, string phone, double matchingPercentage)
        {
            Id = id;
            Phone = phone;
            MatchingPercentage = matchingPercentage;
        }
    }

}
