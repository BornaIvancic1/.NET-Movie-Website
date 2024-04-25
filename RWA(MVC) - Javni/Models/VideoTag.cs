namespace RWA_MVC_JAVNI.Models
{
    public class VideoTag
    {


        public int Id { get; set; }
        public int VideoId { get; set; }
        public int TagId { get; set; }
        public Tag ?Tag { get; set; }
     
    }
}
