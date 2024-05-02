namespace GiftMatchServer.BL
{
    public class Associated
    {
        public int IntrestId { get; set; }
        public string GiftName { get; set; }

        public Associated(int intrestId, string giftName)
        {
            IntrestId = intrestId;
            GiftName = giftName;
        }
    }

}
