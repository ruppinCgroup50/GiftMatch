namespace GiftMatchServer.BL
{
    public class Attributes
    {
        public int Id { get; set; }
        public string AttributesName { get; set; }

        public Attributes(int id, string attributesName)
        {
            Id = id;
            AttributesName = attributesName;
        }
    }

}
