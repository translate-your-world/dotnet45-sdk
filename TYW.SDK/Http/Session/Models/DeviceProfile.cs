namespace TYW.SDK.Http.Session.Models
{
    public class DeviceProfile
    {
        public string accountName { get; set; }

        public string bearerToken { get; set; }

        public string deviceId { get; set; }

        public UserProfileModel profile { get; set; }
    }
}
