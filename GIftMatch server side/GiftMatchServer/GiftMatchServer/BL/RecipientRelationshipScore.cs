namespace GiftMatchServer.BL
{
    public class RecipientRelationshipScore
    {

        public string Phone { get; set; }
        public int RelationshipScore { get; set; }


        public RecipientRelationshipScore()
        {

        }
        public RecipientRelationshipScore( string phone, int relationshipScore)
        {
            Phone = phone;
            RelationshipScore = relationshipScore;
        }
    }
}
}
