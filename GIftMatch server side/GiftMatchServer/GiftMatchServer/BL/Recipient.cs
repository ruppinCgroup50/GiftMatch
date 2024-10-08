﻿using System.Security.Cryptography.Xml;

namespace GiftMatchServer.BL
{
    public class Recipient
    {
       
        public string Name { get; set; }
        public string Gender { get; set; }
        public string RelationType { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public int RelationshipScore { get; set; }
        public string Image { get; set; }
        public string UserEmail { get; set; }

        public Recipient()
        {

        }
        public Recipient( string name, string gender, string relationType, DateTime birthday, string phone, int relationshipScore, string image, string userEmail)
        {
           
            Name = name;
            Gender = gender;
            RelationType = relationType;
            Birthday = birthday;
            Phone = phone;
            RelationshipScore = relationshipScore;
            Image = image;
            UserEmail = userEmail;
            
        }
    }

}
