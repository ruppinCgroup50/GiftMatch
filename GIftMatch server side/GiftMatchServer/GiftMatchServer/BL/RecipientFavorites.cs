namespace GiftMatchServer.BL
{
    public class RecipientFavorites
    {


        string giftName;
        string phone;


        public string GiftName { get => giftName; set => giftName = value; }
        public string Phone { get => phone; set => phone = value; }

        public RecipientFavorites(string giftName, string phone)
        {
            GiftName = giftName;
            Phone = phone;
        }
        public RecipientFavorites()
            {

            }

    }
}
