namespace GiftMatchServer.BL
{
    public class GiftRecipient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string RelationType { get; set; }
        public int Birthday { get; set; }
        public int RelationshipScore { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public int IdBasket { get; set; }

        public GiftRecipient(int id, string name, string gender, string relationType, int birthday, int relationshipScore, string image, string userName, int idBasket)
        {
            Id = id;
            Name = name;
            Gender = gender;
            RelationType = relationType;
            Birthday = birthday;
            RelationshipScore = relationshipScore;
            Image = image;
            UserName = userName;
            IdBasket = idBasket;
        }
    }

}
