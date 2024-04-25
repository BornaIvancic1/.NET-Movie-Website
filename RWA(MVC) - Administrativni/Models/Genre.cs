using System.ComponentModel.DataAnnotations;

namespace RWA_MVC____2.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="This field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Description { get; set; }
      
    }
}
