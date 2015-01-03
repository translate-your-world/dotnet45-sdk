namespace TYW.SDK.Http.Session.Models
{
    public class SessionModel
    {
        public string sessionId { get; set; }

        public string browserId { get; set; }

        public bool isOwner { get; set; }

        public bool canSpeak { get; set; }

        public bool canListen { get; set; }

        public string defaultLayout { get; set; }

        public bool defaultTts { get; set; }

        public SessionCapabilityModel[] capabilities { get; set; }

        public DeviceModel device { get; set; }
    }

    public class SessionCapabilityModel
    {
        public string name { get; set; }

        public string value { get; set; }
    }
}
