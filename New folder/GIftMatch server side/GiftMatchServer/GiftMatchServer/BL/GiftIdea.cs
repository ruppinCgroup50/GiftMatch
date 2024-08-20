namespace GiftMatchServer.BL
{
    public class GiftIdea
    {
        public string GiftName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }

        public GiftIdea(string giftName, string description, int price, string image, string userName)
        {
            GiftName = giftName;
            Description = description;
            Price = price;
            Image = image;
            UserName = userName;
        }

        public GiftIdea()
        {
        }

    }
}
