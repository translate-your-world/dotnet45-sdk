using System;

namespace TYW.SDK.Http.Session.Models
{
    public class AudioModel
    {
        public AudioFileModel[] files { get; set; }

        public class AudioFileModel
        {
            public string text { get; set; }

            public string language { get; set; }

            public string speakerId { get; set; }

            public string audioFile { get; set; }

            public DateTime received { get; set; }

            public string localPath { get; set; }
        }
    }
}
