﻿namespace GiftMatchServer.BL
{
    public class AssociatedInterest
    {
        public int IntrestID { get; set; }
        public string GiftName { get; set; }
        public AssociatedInterest()
        {

        }
        public AssociatedInterest(int id, string giftName)
        {
            IntrestID = id;
            GiftName = giftName;

        }
    }
}
