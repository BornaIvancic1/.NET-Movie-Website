using System.ComponentModel.DataAnnotations;

namespace RWA_MVC_JAVNI.Models
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
