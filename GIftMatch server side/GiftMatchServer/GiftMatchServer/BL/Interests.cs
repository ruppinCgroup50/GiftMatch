namespace GiftMatchServer.BL
{
    public class Interests
    {
        public int Id { get; set; }
        public string InterestName { get; set; }
        public string LogoPath { get; set; }

        public Interests()
        {
     
        }
        public Interests(int id, string interestName,string logoPath)
        {
            Id = id;
            InterestName = interestName;
            LogoPath = logoPath;
        }
    }

}
