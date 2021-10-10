using System.Text.Json.Serialization;

namespace Domain.Clients.Firebase.Models
{
    public class FirebaseSignUpResponse
    {
        [JsonPropertyName("idToken")]
        public string IdToken { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [JsonPropertyName("localId")]
        public string FirebaseId { get; set; }

        public Difficulty Difficulty { get; set; }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}