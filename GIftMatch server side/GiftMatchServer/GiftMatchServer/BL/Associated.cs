namespace GiftMatchServer.BL
{
    public class Associated
    {
        public int Id { get; set; }
        public string GiftName { get; set; }

        public Associated(int id, string giftName)
        {
            Id = id;
            GiftName = giftName;
        }
    }

}
