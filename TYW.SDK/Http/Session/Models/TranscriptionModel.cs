using System;
namespace TYW.SDK.Http.Session.Models
{
    public class TranscriptionModel
    {
        public TranscriptionLineModel[] transcriptions { get; set; }

        public class TranscriptionLineModel
        {
            public string text { get; set; }

            public LanguageStreamModel language { get; set; }

            public string speakerId { get; set; }

            public string speakerName { get; set; }

            public string stamp { get; set; }

            public int ordinal { get; set; }

            public DateTime received { get; set; }
        }
    }
}
