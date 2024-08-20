namespace GiftMatchServer.BL
{
    public class GiftList
    {
        public string GiftName { get; set; }

        public int AttrId { get; set; }


        public GiftList(string giftName, int attrId)
        {
            GiftName = giftName;
            AttrId = attrId;
        }
        public GiftList()
        {

        }
    }
}
