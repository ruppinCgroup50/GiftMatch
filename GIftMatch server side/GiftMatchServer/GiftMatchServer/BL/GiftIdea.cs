namespace GiftMatchServer.BL
{
    public class GiftIdea
    {
        public string GiftName { get; set; }
        public string Description { get; set; }
        public int Budget { get; set; }
        public int Image { get; set; }
        public string UserName { get; set; }

        public GiftIdea(string giftName, string description, int budget, int image, string userName)
        {
            GiftName = giftName;
            Description = description;
            Budget = budget;
            Image = image;
            UserName = userName;
        }
    }
}
