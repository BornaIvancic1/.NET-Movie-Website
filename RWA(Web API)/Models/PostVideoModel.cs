using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RWA_Web_API_.Models
{
    public class PostVideoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

       
        public string? Name { get; set; }
        public string? Description { get; set; }

        public int GenreId { get; set; }
       
      
        public int TotalSeconds { get; set; }
        public string? StreamingUrl { get; set; }

        public int imageId { get; set; }


    }
}
