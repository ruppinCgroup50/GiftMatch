namespace GiftMatchServer.BL
{
    public class Interests
    {
        public int Id { get; set; }
        public string InterestName { get; set; }

        public Interests(int id, string interestName)
        {
            Id = id;
            InterestName = interestName;
        }
    }

}
