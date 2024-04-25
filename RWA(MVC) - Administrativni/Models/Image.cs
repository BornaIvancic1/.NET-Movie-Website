namespace RWA_MVC____2.Models
{
    public class Image
    {
        public string Content { get; set; }
        public int Id { get; set; }
        public Video Video { get; internal set; }
        public int VideoId { get; internal set; }
    }
}
