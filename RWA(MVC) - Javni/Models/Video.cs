﻿namespace RWA_MVC_JAVNI.Models
{
    public class Video
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int GenreId { get; set; }
      
        public int TotalSeconds { get; set; }
        public string StreamingUrl { get; set; }
        public int ImageId{ get; set; }
    
    }
}
