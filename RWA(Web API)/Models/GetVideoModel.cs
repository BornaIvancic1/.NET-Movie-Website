using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RWA_Web_API_.Models
{
    public class GetVideoModel
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
     
        public DateTime CreatedAt { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    
        public int GenreId { get; set; }
        [NotMapped]
        public Genre? Genre { get; set; }
        [NotMapped]
        public Image? Image { get; set; }
        public int TotalSeconds { get; set; }
        public string? StreamingUrl { get; set; }
       
        public int imageId { get; set; }

       
    }
}
