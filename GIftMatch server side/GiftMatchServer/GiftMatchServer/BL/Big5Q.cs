namespace GiftMatchServer.BL
{
    public class Big5Q
    {
     

        public int Id { get; set; }
        public string Qname { get; set; }
        public int AttId { get; set; }

        public Big5Q()
        {
            
        }
        public Big5Q(int id, string qname, int attId)
        {
            Id = id;
            Qname = qname;
            AttId = attId;
        }
    }
}
