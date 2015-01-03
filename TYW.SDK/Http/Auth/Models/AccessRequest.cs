namespace TYW.SDK.Http.Auth.Models
{
    public class AccessRequest
    {
        public string account { get; set; }

        public string clientId { get; set; }

        public string clientSecret { get; set; }

        public string code { get; set; }

        public string grantType { get; set; }
    }
}
