using Newtonsoft.Json;

namespace AuebUnofficial.Model
{
    public class EclassUser
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("uid")]
        public string Uid { get; set; }
        [JsonProperty("remember")]
        public bool IsRememberEnabled { get; set; }
        public EclassUser() { }
    }
}
