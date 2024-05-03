namespace GiftMatchServer.BL
{
    public class Big5Q
    {
     

        public int Id { get; set; }
        public string Qname { get; set; }
        public Big5Q()
        {
            
        }
        public Big5Q(int id, string qname)
        {
            Id = id;
            Qname = qname;
        }
    }
}
