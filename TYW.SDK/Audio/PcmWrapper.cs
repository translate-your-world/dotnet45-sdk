using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TYW.SDK.Audio
{
    public class PcmWrapper
    {
        private object _lock = new Object();

        public string ChunkID { get; set; }

        public int ChunkSize { get; set; }

        public string Format { get; set; }

        public string SubChunk1ID { get; set; }

        public int SubChunk1Size { get; set; }

        public short AudioFormat { get; set; }

        public short NumChannels { get; set; }

        public int SampleRate { get; set; }

        public int ByteRate { get; set; }

        public short BlockAlign { get; set; }

        public short BitsPerSample { get; set; }

        public string SubChunk2ID { get; set; }

        public int SubChunk2Size { get; set; }

        protected byte[] Payload { get; set; }

        public float Seconds { get { return (float)this.Payload.Length / (float)this.ByteRate; } }

        public PcmWrapper(PcmWrapper source)
        {
            EncodeHeader(source.GetHeader());
            Payload = new byte[source.Payload.Length];
            Buffer.BlockCopy(source.Payload, 0, Payload, 0, source.Payload.Length);
        }

        public PcmWrapper(byte[] content)
        {
            EncodeHeader(content);
            EncodePayload(content);
        }

        public PcmWrapper(byte[] content, int sampleRate, short numChannels, short bitsPerSample)
        {
            ChunkID = "RIFF";
            ChunkSize = 36 + content.Length;
            Format = "WAVE";
            SubChunk1ID = "fmt ";
            SubChunk1Size = 16;
            AudioFormat = 1;
            NumChannels = numChannels;
            SampleRate = sampleRate;
            ByteRate = sampleRate * numChannels * bitsPerSample / 8;
            BlockAlign = (short)(numChannels * bitsPerSample / 8);
            BitsPerSample = bitsPerSample;
            SubChunk2ID = "data";
            SubChunk2Size = content.Length;

            Payload = new byte[content.Length];
            Array.Copy(content, 0, Payload, 0, content.Length);
        }

        public void Combine(PcmWrapper source)
        {
            lock (_lock)
            {
                byte[] payload = new byte[Payload.Length + source.Payload.Length];
                Buffer.BlockCopy(Payload, 0, payload, 0, Payload.Length);
                Buffer.BlockCopy(source.Payload, 0, payload, Payload.Length, source.Payload.Length);

                Payload = payload;
                SubChunk2Size = payload.Length;
                ChunkSize = 36 + payload.Length;
            }
        }

        public byte[] Encode()
        {
            lock (_lock)
            {
                byte[] bytes = new byte[44 + Payload.Length];
                byte[] header = GetHeader();
                Buffer.BlockCopy(header, 0, bytes, 0, 44);
                Buffer.BlockCopy(Payload, 0, bytes, 44, Payload.Length);
                return bytes;
            }
        }

        public byte[] GetPayload()
        {
            byte[] ret;
            lock (_lock)
            {
                ret = new byte[Payload.Length];
                Buffer.BlockCopy(Payload, 0, ret, 0, Payload.Length);
            }
            return ret;
        }

        public byte[] Splice(int packetNumber, int tolerance)
        {
            int tolerantLimit = packetNumber - tolerance;
            if (tolerantLimit < 0) tolerantLimit = 0;

            byte[] content = new byte[packetNumber];
            byte[] newContent = new byte[Payload.Length - tolerantLimit];

            lock (_lock)
            {
                Buffer.BlockCopy(Payload, 0, content, 0, packetNumber);
                Buffer.BlockCopy(Payload, tolerantLimit, newContent, 0, Payload.Length - tolerantLimit);
                Payload = newContent;
            }

            return content;
        }

        protected void EncodeHeader(byte[] content)
        {
            try
            {
                ChunkID = PcmUtilities.Encoding.GetString(content, 0, 4);
                ChunkSize = BitConverter.ToInt32(content, 4);
                Format = PcmUtilities.Encoding.GetString(content, 8, 4);
                SubChunk1ID = PcmUtilities.Encoding.GetString(content, 12, 4);
                SubChunk1Size = BitConverter.ToInt32(content, 16);
                AudioFormat = BitConverter.ToInt16(content, 20);
                NumChannels = BitConverter.ToInt16(content, 22);
                SampleRate = BitConverter.ToInt32(content, 24);
                ByteRate = BitConverter.ToInt32(content, 28);
                BlockAlign = BitConverter.ToInt16(content, 32);
                BitsPerSample = BitConverter.ToInt16(content, 34);
                SubChunk2ID = PcmUtilities.Encoding.GetString(content, 36, 4);
                SubChunk2Size = BitConverter.ToInt32(content, 40);
            }
            catch (Exception exc)
            {
                throw new Exception("Content length: " + content.Length, exc);
            }
        }

        protected void EncodePayload(byte[] content)
        {
            byte[] payload = new byte[content.Length - 44];
            Array.Copy(content, 44, payload, 0, content.Length - 44);
            Payload = payload;
        }

        protected byte[] GetHeader()
        {
            byte[] encoded = new byte[44];
            Buffer.BlockCopy(PcmUtilities.Encoding.GetBytes(ChunkID), 0, encoded, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(Payload.Length + 36), 0, encoded, 4, 4);
            Buffer.BlockCopy(PcmUtilities.Encoding.GetBytes(Format), 0, encoded, 8, 4);
            Buffer.BlockCopy(PcmUtilities.Encoding.GetBytes(SubChunk1ID), 0, encoded, 12, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(SubChunk1Size), 0, encoded, 16, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(AudioFormat), 0, encoded, 20, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(NumChannels), 0, encoded, 22, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(SampleRate), 0, encoded, 24, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(ByteRate), 0, encoded, 28, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(BlockAlign), 0, encoded, 32, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(BitsPerSample), 0, encoded, 34, 2);
            Buffer.BlockCopy(PcmUtilities.Encoding.GetBytes(SubChunk2ID), 0, encoded, 36, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(Payload.Length), 0, encoded, 40, 4);
            return encoded;
        }
    }
}
