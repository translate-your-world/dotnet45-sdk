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
        public TywiAudioStream(Stream stream) : base(stream) { }

        public void WriteAudio(byte[] audio)
        {
            _baseStream.Write(audio, (int)_baseStream.Length, audio.Length);
        }

        public byte[] ReadAudio()
        {
            byte[] buffer;

            lock (_lock)
            {
                buffer = new byte[_baseStream.Length];
                _baseStream.Read(buffer, 0, buffer.Length);
            }

            return buffer;
        }
    }
}
