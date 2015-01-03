namespace TYW.SDK.Http.Session.Models
{
    public class DeviceModel
    {
        public string token { get; set; }

        public float phraseLength { get; set; }

        public LanguageStreamModel[] textLanguages { get; set; }

        public LanguageStreamModel[] voiceLanguages { get; set; }

        public UserProfileModel[] users { get; set; }

        public bool isMaster { get; set; }
    }
}
