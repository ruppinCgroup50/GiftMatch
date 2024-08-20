namespace GiftMatchServer.BL
{
    public class GiftListInterest
    {
        public string GiftName { get; set; }

        public string InterestName { get; set; }


        public GiftListInterest(string giftName, string interestName)
        {
            GiftName = giftName;
            InterestName = interestName;
        }
        public GiftListInterest()
        {

        }
    }
}
