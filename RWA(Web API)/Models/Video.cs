using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RWA_Web_API_.Models
{
    public class Video
    {
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
         [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        public string ?Name { get; set; }
        public string ?Description { get; set; }
        [JsonIgnore]
        public int GenreId { get; set; }
        [NotMapped]
        public Genre ?Genre { get; set; }
        [NotMapped]
        public Image ?Image { get; set; }         
        public int TotalSeconds { get; set; }
        public string ?StreamingUrl { get; set; }
        [JsonIgnore]
        public int imageId{ get; set; }
 
    }
}
