using System.Text.Json.Serialization;

namespace RWA_Web_API_.Models
{
    public class PostNotification
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? ReceiverEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
     
    }
}
