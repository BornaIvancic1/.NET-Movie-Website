using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace RWA_MVC____2.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
       
        public DateTime? DeletedAt { get; set; }
        [Required]
        public string ?Username { get; set; }
        [Required]
        public string ?FirstName { get; set; }
        [Required]
        public string ?LastName { get; set; }
        [Required]
        [EmailAddress]
        public string ?Email { get; set; }
        public string ?PwdHash { get; set; }
        public string ?PwdSalt { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string ?Phone { get; set; }
        public bool IsConfirmed { get; set; }
        public string ?SecurityToken { get; set; }
        public int CountryOfResidenceId { get; set; }
        public Country ?CountryOfResidence { get; set; }


    }
}
