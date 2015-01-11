using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.IO;
using TYW.SDK.Audio;
using TYW.SDK.Models;
using System.Linq;
using System;
using System.Net;
using System.Collections.Generic;

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
            byte[] resampled = PcmUtilities.Resample(buffer, _waveIn.WaveFormat.SampleRate, (short)_waveIn.WaveFormat.BitsPerSample, (short)_waveIn.WaveFormat.Channels,
                16000, 16, 1);
            this.WriteAudio(resampled);
        }
    }

    public class AudioOutStream : TywiAudioStream
    {
        public bool IsPlaying { get; set; }

        private WaveOut _waveout;
        private Queue<Stream> _audio = new Queue<Stream>();
        private object _lock = new object();

        public AudioOutStream(MemoryStream stream)
            : base(stream)
        {
            _waveout = new WaveOut();
        }

        public override void QueueAudio(string url)
        {
            lock (_lock)
            {
                EnqueueAudio(url);
            }

            if (!IsPlaying) 
            {
                IsPlaying = true;
                PlayAudio();
            }
        }

        private void EnqueueAudio(string url)
        {
            Stream ms = new MemoryStream();
            using (Stream stream = WebRequest.Create(url).GetResponse().GetResponseStream())
            {
                byte[] buffer = new byte[32768];
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
            }
            _audio.Enqueue(ms);
        }

        private void PlayAudio()
        {
            Stream ms = _audio.Dequeue();
            ms.Position = 0;                
            var reader = new Mp3FileReader(ms);
            _waveout.Init(reader);
            _waveout.Play();
            _waveout.PlaybackStopped += NextAudio;
        }

        private void NextAudio(object sender, StoppedEventArgs e)
        {
            if (_audio.Count > 0) 
            {
                PlayAudio();
            }
            else
            {
                IsPlaying = false;
            }
        }
    }

}
