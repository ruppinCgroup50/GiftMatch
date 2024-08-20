namespace GiftMatchServer.BL
{
    public class Contains
    {
        public int IdBasket { get; set; }
        public int GiftName { get; set; }

        public Contains(int idBasket, int giftName)
        {
            IdBasket = idBasket;
            GiftName = giftName;
        }
    }

}
