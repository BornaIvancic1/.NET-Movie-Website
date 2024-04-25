using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace RWA_Web_API_.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime DeletedAt { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [JsonPropertyName("password")]
        public string? PwdHash { get; set; }
        [JsonIgnore]
        public string? PwdSalt { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string ?Phone { get; set; }
        [JsonIgnore]
        public bool IsConfirmed { get; set; }
        [JsonIgnore]
        public string ?SecurityToken { get; set; }
        public int CountryOfResidenceId { get; set; }
  
  
        [JsonIgnore]
        [NotMapped]
        public string ?VerificationToken { get;  set; }
    
    }
}
