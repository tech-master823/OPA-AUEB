using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuebUnofficial.Core.Model
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
