using NAudio.Wave;
using System.IO;
using TYW.SDK.Models;

namespace TYW.SDK.Sample.Audio
{
    public class AudioInStream : TywiAudioStream
    {
        private WaveIn _waveIn;

        public bool IsActive { get; set; }

        public AudioInStream(WaveIn waveIn, MemoryStream stream) : base(stream) 
        {
            _waveIn = waveIn;
            _waveIn.DataAvailable += OnDataAvailable;
        }

        public void StartRecording()
        {
            _waveIn.StartRecording();
        }

        public void StopRecording()
        {
            _waveIn.StopRecording();
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;
            this.WriteAudio(buffer);
        }
    }
}
