using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NAudio.Wave;

namespace TYW.SDK.Audio
{
    public class PcmUtilities
    {
        public static Encoding Encoding = System.Text.Encoding.ASCII;

        public static string GetWavHeaderDetails(byte[] encoded)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("=============================================");
            builder.AppendLine("ChunkID: " + Encoding.GetString(encoded, 0, 4));
            builder.AppendLine("ChunkSize: " + BitConverter.ToInt32(encoded, 4));
            builder.AppendLine("Format: " + Encoding.GetString(encoded, 8, 4));
            builder.AppendLine("SubChunk1ID: " + Encoding.GetString(encoded, 12, 4));
            builder.AppendLine("SubChunk1Size: " + BitConverter.ToInt32(encoded, 16));
            builder.AppendLine("AudioFormat: " + BitConverter.ToInt16(encoded, 20));
            builder.AppendLine("NumChannels: " + BitConverter.ToInt16(encoded, 22));
            builder.AppendLine("SampleRate: " + BitConverter.ToInt32(encoded, 24));
            builder.AppendLine("ByteRate: " + BitConverter.ToInt32(encoded, 28));
            builder.AppendLine("BlockAlign: " + BitConverter.ToInt16(encoded, 32));
            builder.AppendLine("BitsPerSample: " + BitConverter.ToInt16(encoded, 34));
            builder.AppendLine("SubChunk2ID: " + Encoding.GetString(encoded, 36, 4));
            builder.AppendLine("SubChunk2Size: " + BitConverter.ToInt32(encoded, 40));
            builder.AppendLine("Total Length: " + encoded.Length);
            builder.AppendLine("=============================================");
            return builder.ToString();
        }

        public static int? GetFirstSilence(PcmWrapper pcm, float startSecond, float endSecond,
            float minPauseLength, float maxPauseLength, int noiseLevel)
        {
            int lowFrequency = -noiseLevel;
            int highFrequency = noiseLevel;

            int startSearch = (int)(startSecond * pcm.ByteRate);
            if (startSearch < 0) startSearch = 0;
            startSearch += startSearch % 2;
            int endSearch = (int)(endSecond * pcm.ByteRate);
            int minSilenceThreshold = (int)(minPauseLength * pcm.ByteRate);
            int maxSilenceThreshold = (int)(maxPauseLength * pcm.ByteRate);
            byte[] payload = pcm.GetPayload();

            if (minPauseLength == 0)
            {
                int ret = startSearch;
                ret -= (ret % 2);
                return ret;
            }

            int silenceStart = 0;
            int silenceCount = 0;
            float maxSilence = 0f;
            List<short> shorts = new List<short>();

            for (int j = startSearch; j < endSearch && j < payload.Length - 1; j += 2)
            {
                short snd = ComplementToSigned(j, payload);

                if (silenceCount >= maxSilenceThreshold)
                {
                    int ret = silenceStart + (silenceCount / 2);
                    ret -= (ret % 2);
                    return ret;
                }
                else if (snd > lowFrequency && snd < highFrequency)
                {
                    if (silenceCount == 0)
                    {
                        silenceStart = j;
                    }

                    silenceCount += 2;
                }
                else if (silenceCount >= minSilenceThreshold)
                {
                    int ret = silenceStart + (silenceCount / 2);
                    ret -= (ret % 2);
                    return ret;
                }
                else
                {
                    maxSilence = Math.Max(silenceCount, maxSilence);
                    silenceCount = 0;
                }

                shorts.Add(snd);
            }
            return null;
        }

        public static short ComplementToSigned(int intPos, byte[] content)
        {
            try
            {
                short snd = BitConverter.ToInt16(content, intPos);
                if (snd != 0)
                    snd = Convert.ToInt16((~snd | 1));
                return snd;
            }
            catch (Exception exc)
            {
                throw new Exception("Position: " + intPos + ", Length" + content.Length, exc);
            }
        }

        public static int ComplementToSigned32(int intPos, byte[] content)
        {
            try
            {
                int snd = BitConverter.ToInt32(content, intPos);
                if (snd != 0)
                    snd = Convert.ToInt32((~snd | 1));
                return snd;
            }
            catch (Exception exc)
            {
                throw new Exception("Position: " + intPos + ", Length" + content.Length, exc);
            }
        }


        public static void SaveFile(PcmWrapper data, string saveName)
        {
            byte[] save = data.Encode();
            FileInfo info = new FileInfo(saveName);

            if (!info.Directory.Exists)
                info.Directory.Create();

            using (BinaryWriter writer = new BinaryWriter(new FileStream(saveName, FileMode.Create)))
            {
                writer.Write(save);
            }
        }

        public static byte[] Resample(byte[] pcm, int fromRate, short fromDepth, short fromChannels, int toRate, short toDepth, short toChannels)
        {
            using (MemoryStream mem = new MemoryStream(pcm))
            {
                using (RawSourceWaveStream stream = new RawSourceWaveStream(mem, new WaveFormat(fromRate, fromDepth, fromChannels)))
                {
                    var outFormat = new WaveFormat(toRate, stream.WaveFormat.Channels);
                    using (var resampler = new WaveFormatConversionStream(outFormat, stream))
                    {

                        int resampled_length = (int)((float)pcm.Length * ((float)toRate / (float)fromRate));

                        byte[] ret = new byte[resampled_length];
                        resampler.Read(ret, 0, resampled_length);
                        return ret;
                    }
                }
            }
        }

    }
}
