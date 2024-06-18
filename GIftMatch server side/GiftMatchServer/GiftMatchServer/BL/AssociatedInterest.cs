namespace GiftMatchServer.BL
{
    public class AssociatedInterest
    {
        public int IntrestID { get; set; }
        public string GiftName { get; set; }
        public int Priority { get; set; }
        public AssociatedInterest()
        {

        }
        public AssociatedInterest(int id, string giftName,int priority)
        {
            IntrestID = id;
            GiftName = giftName;
            Priority = priority;

        }
    }
}
