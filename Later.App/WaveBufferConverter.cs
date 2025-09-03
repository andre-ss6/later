using NAudio.Wave;

namespace Later.App;

internal static class WaveBufferConverter
{
    // Convert interleaved bytes -> interleaved floats [-1..1]
    public static void BytesToFloats(byte[] buffer, int bytesRecorded, WaveFormat wf, float[] dest)
    {
        int channels = wf.Channels;
        int bps = wf.BitsPerSample;
        int bytesPerSample = bps / 8;
        int frameCount = bytesRecorded / (bytesPerSample * channels);

        int src = 0;
        int dst = 0;

        if (wf.Encoding == WaveFormatEncoding.IeeeFloat && bps == 32)
        {
            for (int f = 0; f < frameCount; f++)
            {
                for (int c = 0; c < channels; c++)
                {
                    dest[dst++] = BitConverter.ToSingle(buffer, src);
                    src += 4;
                }
            }
            return;
        }

        switch (bps)
        {
            case 8:
                for (int i = 0; i < frameCount * channels; i++)
                {
                    byte b = buffer[src++];
                    dest[dst++] = (b - 128) / 128f; // unsigned 8-bit PCM -> centered
                }
                break;

            case 16:
                for (int i = 0; i < frameCount * channels; i++)
                {
                    short val = (short)(buffer[src] | (buffer[src + 1] << 8));
                    src += 2;
                    dest[dst++] = val / 32768f;
                }
                break;

            case 24:
                for (int i = 0; i < frameCount * channels; i++)
                {
                    int val = buffer[src] | (buffer[src + 1] << 8) | (buffer[src + 2] << 16);
                    // sign extend 24->32
                    if ((val & 0x800000) != 0) val |= unchecked((int)0xFF000000);
                    src += 3;
                    dest[dst++] = val / 8388608f; // 2^23
                }
                break;

            case 32:
                if (wf.Encoding == WaveFormatEncoding.Pcm)
                {
                    for (int i = 0; i < frameCount * channels; i++)
                    {
                        int val = BitConverter.ToInt32(buffer, src);
                        src += 4;
                        dest[dst++] = val / 2147483648f; // 2^31
                    }
                }
                else
                {
                    // Other 32-bit encodings (handled earlier if IEEE float)
                    for (int i = 0; i < frameCount * channels; i++)
                    {
                        dest[dst++] = BitConverter.ToSingle(buffer, src);
                        src += 4;
                    }
                }
                break;

            case 64:
                if (wf.Encoding == WaveFormatEncoding.IeeeFloat)
                {
                    for (int i = 0; i < frameCount * channels; i++)
                    {
                        double d = BitConverter.ToDouble(buffer, src);
                        src += 8;
                        dest[dst++] = (float)d;
                    }
                }
                else throw new NotSupportedException("Unsupported 64-bit encoding");
                break;

            default:
                throw new NotSupportedException($"Unsupported bits per sample: {bps}");
        }
    }

    // Convert floats back into bytes matching wf. Clamping is performed.
    public static void FloatsToBytes(float[] source, int sampleCount, WaveFormat wf, byte[] dest)
    {
        int bps = wf.BitsPerSample;
        int src = 0;
        int dst = 0;

        if (wf.Encoding == WaveFormatEncoding.IeeeFloat && bps == 32)
        {
            for (int i = 0; i < sampleCount; i++)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(source[src++]), 0, dest, dst, 4);
                dst += 4;
            }
            return;
        }

        switch (bps)
        {
            case 8:
                for (int i = 0; i < sampleCount; i++)
                {
                    float f = Math.Max(-1f, Math.Min(1f, source[src++]));
                    byte b = (byte)(f * 127f + 128f);
                    dest[dst++] = b;
                }
                break;

            case 16:
                for (int i = 0; i < sampleCount; i++)
                {
                    float f = Math.Max(-1f, Math.Min(1f, source[src++]));
                    int s = (int)(f * 32767f);
                    if (s > short.MaxValue) s = short.MaxValue;
                    if (s < short.MinValue) s = short.MinValue;
                    dest[dst++] = (byte)(s & 0xFF);
                    dest[dst++] = (byte)((s >> 8) & 0xFF);
                }
                break;

            case 24:
                for (int i = 0; i < sampleCount; i++)
                {
                    float f = Math.Max(-1f, Math.Min(1f, source[src++]));
                    int s = (int)(f * 8388607f);
                    dest[dst++] = (byte)(s & 0xFF);
                    dest[dst++] = (byte)((s >> 8) & 0xFF);
                    dest[dst++] = (byte)((s >> 16) & 0xFF);
                }
                break;

            case 32:
                if (wf.Encoding == WaveFormatEncoding.Pcm)
                {
                    for (int i = 0; i < sampleCount; i++)
                    {
                        float f = Math.Max(-1f, Math.Min(1f, source[src++]));
                        int s = (int)(f * 2147483647f);
                        dest[dst++] = (byte)(s & 0xFF);
                        dest[dst++] = (byte)((s >> 8) & 0xFF);
                        dest[dst++] = (byte)((s >> 16) & 0xFF);
                        dest[dst++] = (byte)((s >> 24) & 0xFF);
                    }
                }
                else
                {
                    for (int i = 0; i < sampleCount; i++)
                    {
                        Buffer.BlockCopy(BitConverter.GetBytes(source[src++]), 0, dest, dst, 4);
                        dst += 4;
                    }
                }
                break;

            default:
                throw new NotSupportedException($"Unsupported bits per sample: {bps}");
        }
    }
}
