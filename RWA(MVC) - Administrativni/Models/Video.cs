namespace RWA_MVC____2.Models
{
    public class Video
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int TotalSeconds { get; set; }
        public string StreamingUrl { get; set; }
        public int ImageId{ get; set; }
    
    }
}
