using System.Text.Json.Serialization;

namespace RWA_Web_API_.Models
{
    public class Tag
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string ?Name { get; set; }
    }
}
