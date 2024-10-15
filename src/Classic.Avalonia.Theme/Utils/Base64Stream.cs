using System;
using System.IO;

namespace Classic.Avalonia.Theme.Utils;

internal class Base64Stream : Stream
{
    private readonly byte[] buffer;
    private int position;

    public Base64Stream(string base64)
    {
        buffer = Convert.FromBase64String(base64);
    }

    public override void Flush()
    {
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        count = Math.Min(count, buffer.Length - position);
        this.buffer.AsSpan(position, count).CopyTo(buffer.AsSpan(offset));
        position += count;
        return count;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                position = (int)offset;
                break;
            case SeekOrigin.Current:
                position += (int)offset;
                break;
            case SeekOrigin.End:
                position = buffer.Length - (int)offset;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
        }

        return position;
    }

    public override void SetLength(long value)
    {
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
    }

    public override bool CanRead => true;
    public override bool CanSeek => true;
    public override bool CanWrite => false;
    public override long Length => buffer.Length;

    public override long Position
    {
        get => position;
        set => position = (int)value;
    }
}