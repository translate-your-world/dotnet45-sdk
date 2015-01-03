namespace TYW.SDK.Http.Session.Models
{
    public class UserProfileModel
    {
        public string id { get; set; }

        public int ordinal { get; set; }

        public string name { get; set; }

        public string language { get; set; }

        public string region { get; set; }

        public string dialect { get; set; }

        public LanguageStreamModel textLanguage { get; set; }

        public LanguageStreamModel audioLanguage { get; set; }

        public string browser { get; set; }

        public bool voipRequired { get; set; }

        public string voipStatus { get; set; }

        public string voipAccount { get; set; }

        public string audioSource { get; set; }
    }

}
