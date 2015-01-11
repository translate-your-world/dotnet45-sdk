using System;
using System.IO;
using TYW.SDK.Http;

namespace TYW.SDK.Models
{
    public class TywiStream
    {
        protected object _lock = new object();
        protected Stream _baseStream;

        public TywiStream(Stream stream)
        {            
            _baseStream = Stream.Synchronized(stream);
        }
    }

    public delegate void TextEventHandler(string text);

    public class TywiTextStream : TywiStream
    {        
        protected long readPosition = 0;

        public TywiTextStream(Stream stream) : base(stream) { }

        public event TextEventHandler TextUpdated;

        public void WriteText(string text)
        {
            lock (_lock)
            {
                if (text != null)
                {
                    byte[] bytes = HttpUtilities.EncodeString(text);
                    _baseStream.Write(bytes, 0, bytes.Length);
                    if (TextUpdated != null) TextUpdated(text);
                }
            }
        }

        public string ReadText()
        {
            lock (_lock)
            {
                long endPosition = _baseStream.Length;
                if (endPosition > readPosition)
                {
                    byte[] buffer;

                    buffer = new byte[endPosition - readPosition];
                    _baseStream.Read(buffer, 0, (int)(endPosition - readPosition));
                    readPosition = endPosition;

                    return HttpUtilities.ByteArrayToString(buffer);

                }
                else
                {
                    return null;
                }
            }

        }
    }

    public class TywiAudioStream : TywiStream
    {
        protected long readPosition = 0;
        protected byte[] stored;

        public TywiAudioStream(Stream stream) : base(stream) { }

        public void WriteAudio(byte[] audio)
        {
            lock (_lock)
            {
                if (stored == null)
                {
                    stored = new byte[audio.Length];
                    Array.Copy(audio, stored, audio.Length);
                }
                else
                {
                    int startLength = stored.Length;
                    Array.Resize<byte>(ref stored, startLength + audio.Length);
                    Array.Copy(audio, 0, stored, startLength, audio.Length);
                }
            }
        }

        public byte[] ReadAudio()
        {
            lock (_lock)
            {
                byte[] buffer = stored;
                stored = null; 
                return buffer;
            }
        }

        public virtual void QueueAudio(string audio)
        {

        }
    }
}
