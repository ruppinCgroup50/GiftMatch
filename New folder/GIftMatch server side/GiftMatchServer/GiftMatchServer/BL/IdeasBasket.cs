namespace GiftMatchServer.BL
{
    public class IdeasBasket
    {
        public int Id { get; set; }
        public int IdRecipient { get; set; }

        public IdeasBasket(int id, int idRecipient)
        {
            Id = id;
            IdRecipient = idRecipient;
        }
    }

}
