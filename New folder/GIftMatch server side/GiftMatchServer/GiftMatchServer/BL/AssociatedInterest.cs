namespace GiftMatchServer.BL
{
    public class AssociatedInterest
    {
        public int IntrestID { get; set; }
        public string Phone { get; set; }
        public int Priority { get; set; }
        public AssociatedInterest()
        {

        }
        public AssociatedInterest(int id, string phone,int priority)
        {
            IntrestID = id;
            Phone = phone;
            Priority = priority;

        }
    }
}
