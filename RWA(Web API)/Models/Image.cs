using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RWA_Web_API_.Models
{
    public class Image
    {
        [JsonIgnore]
     
        public int Id { get; set; }

        public string ?Content { get; set; }
  
    }
}
