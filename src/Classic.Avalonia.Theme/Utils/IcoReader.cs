using System;
using System.Buffers.Binary;
using System.IO;

namespace Classic.Avalonia.Theme.Utils;

internal readonly struct IcoIcon
{
    private readonly byte[] data;
    public readonly IconEntry[] Icons;

    /// <summary>
    /// Parses the ICO stream and returns icon entries and the raw data buffer.
    /// </summary>
    /// <param name="stream">ICO file stream</param>
    /// <returns>Tuple of icon entries array and the raw data buffer</returns>
    /// <exception cref="InvalidDataException">If the stream is not a valid ICO file</exception>
    public static bool TryParse(Stream stream, out IcoIcon icon)
    {
        icon = default;
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        var data = ms.ToArray();

        var reader = new LittleEndianReader(data);
        ushort reserved = reader.ReadUInt16();
        ushort type = reader.ReadUInt16();
        ushort count = reader.ReadUInt16();

        if (reserved != 0 || type != 1 || count == 0)
            return false;

        var icons = new IconEntry[count];
        for (int i = 0; i < count; i++)
        {
            byte width = reader.ReadByte();
            byte height = reader.ReadByte();
            byte colorCount = reader.ReadByte();
            byte reservedEntry = reader.ReadByte();
            ushort planes = reader.ReadUInt16();
            ushort bpp = reader.ReadUInt16();
            uint size = reader.ReadUInt32();
            uint offset = reader.ReadUInt32();
            icons[i] = new IconEntry(width, height, colorCount, reservedEntry, planes, bpp, size, offset);
        }
        icon = new IcoIcon(icons, data);
        return true;
    }

    /// <summary>
    /// Constructs an IcoReader from an array of icon entries.
    /// </summary>
    private IcoIcon(IconEntry[] icons, byte[] bytes)
    {
        Icons = icons ?? throw new ArgumentNullException(nameof(icons));
        data = bytes ?? throw new ArgumentNullException(nameof(bytes));
    }

    /// <summary>
    /// Gets an icon by its index
    /// </summary>
    public IconEntry GetIcon(int index)
    {
        if ((uint)index >= (uint)Icons.Length)
            throw new ArgumentOutOfRangeException(nameof(index));
        return Icons[index];
    }

    /// <summary>
    /// Gets the icon closest to the specified size
    /// </summary>
    public IconEntry GetIconBySize(int size)
    {
        if (Icons.Length == 0)
            throw new InvalidOperationException("No icons available");
        IconEntry best = Icons[0];
        int bestScore = Math.Abs(best.ActualWidth - size);
        foreach (var icon in Icons)
        {
            int score = Math.Abs(icon.ActualWidth - size);
            if (score < bestScore || (score == bestScore && icon.BitsPerPixel > best.BitsPerPixel))
            {
                best = icon;
                bestScore = score;
            }
        }
        return best;
    }

    /// <summary>
    /// Gets the icon with the highest bit depth at the specified size
    /// </summary>
    public IconEntry? GetIconExactSize(int size)
    {
        IconEntry? best = null;
        foreach (var icon in Icons)
        {
            if (icon.ActualWidth == size)
            {
                if (best == null || icon.BitsPerPixel > best.Value.BitsPerPixel)
                    best = icon;
            }
        }
        return best;
    }

    /// <summary>
    /// Returns a span with the binary data of a particular image.
    /// </summary>
    /// <param name="icon">The icon entry</param>
    /// <param name="data">The ICO file data buffer</param>
    /// <returns>ReadOnlySpan with the image data</returns>
    public ReadOnlyMemory<byte> GetImageData(in IconEntry icon)
    {
        if (icon.DataOffset + icon.DataSize > (uint)data.Length)
            throw new InvalidDataException("Image data out of bounds");
        return new ReadOnlyMemory<byte>(data, (int)icon.DataOffset, (int)icon.DataSize);
    }

    public bool IsBmp(in IconEntry icon)
    {
        var imageData = GetImageData(icon);
        var signature = BinaryPrimitives.ReadUInt32LittleEndian(imageData.Slice(1, 4).Span);
        if (signature == 0x0D474E50)
        {
            return false; // PNG signature
        }

        return true;
    }

    public Stream GetImageStream(in IconEntry icon)
    {
        if (icon.DataOffset + icon.DataSize > (uint)data.Length)
            throw new InvalidDataException("Image data out of bounds");
        return new MemoryStream(data, (int)icon.DataOffset, (int)icon.DataSize, false);
    }

    public readonly struct IconEntry
    {
        public byte Width { get; }
        public byte Height { get; }
        public byte ColorCount { get; }
        public byte Reserved { get; }
        public ushort ColorPlanes { get; }
        public ushort BitsPerPixel { get; }
        public uint DataSize { get; }
        public uint DataOffset { get; }

        public int ActualWidth => Width == 0 ? 256 : Width;
        public int ActualHeight => Height == 0 ? 256 : Height;

        public IconEntry(byte width, byte height, byte colorCount, byte reserved, ushort planes, ushort bpp, uint size, uint offset)
        {
            Width = width;
            Height = height;
            ColorCount = colorCount;
            Reserved = reserved;
            ColorPlanes = planes;
            BitsPerPixel = bpp;
            DataSize = size;
            DataOffset = offset;
        }

        public override string ToString() =>
            $"{ActualWidth}x{ActualHeight}, {BitsPerPixel} bpp, {DataSize} bytes";
    }

    private struct LittleEndianReader
    {
        private readonly byte[] _buffer;
        private int _pos;

        public LittleEndianReader(byte[] buffer)
        {
            _buffer = buffer;
            _pos = 0;
        }

        public byte ReadByte() => _buffer[_pos++];

        public ushort ReadUInt16()
        {
            ushort val = BinaryPrimitives.ReadUInt16LittleEndian(_buffer.AsSpan(_pos, 2));
            _pos += 2;
            return val;
        }

        public uint ReadUInt32()
        {
            uint val = BinaryPrimitives.ReadUInt32LittleEndian(_buffer.AsSpan(_pos, 4));
            _pos += 4;
            return val;
        }
    }
}

internal struct BitmapHeader
{
    public BitmapHeader(ReadOnlySpan<byte> headerBytes)
    {
        var signature = BinaryPrimitives.ReadUInt16LittleEndian(headerBytes.Slice(0, 2));
        if (signature != 0x4D42) // 'BM' signature
            throw new InvalidDataException("Not a valid BMP file");
        SizeInBytes = BinaryPrimitives.ReadInt32LittleEndian(headerBytes.Slice(2, 4));
        Reserved1 = BinaryPrimitives.ReadUInt16LittleEndian(headerBytes.Slice(6, 2));
        Reserved2 = BinaryPrimitives.ReadUInt16LittleEndian(headerBytes.Slice(8, 2));
        PixelDataOffset = BinaryPrimitives.ReadInt32LittleEndian(headerBytes.Slice(10, 4));
    }

    public void WriteTo(Span<byte> buffer)
    {
        if (buffer.Length < 14)
            throw new ArgumentException("Buffer too small for BitmapHeader", nameof(buffer));

        BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(0, 2), 0x4D42); // 'BM' signature
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(2, 4), SizeInBytes);
        BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(6, 2), Reserved1);
        BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(8, 2), Reserved2);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(10, 4), PixelDataOffset);
    }

    public int HeaderBytesLength => 14;
    public int SizeInBytes { get; set; }
    public ushort Reserved1 { get; set; }
    public ushort Reserved2 { get; set; }
    public int PixelDataOffset { get; set; }
}

internal struct BitmapPalette
{
    public unsafe BitmapPalette(BitmapInfoHeader header, ReadOnlySpan<byte> paletteBytes)
    {
        if (header.BitsPerPixel > 8)
        {
            ColorsCount = 0;
            return; // No palette for high color depths
        }

        if (header.ColorsUsed == 0)
            ColorsCount = 1 << header.BitsPerPixel; // Default to 2^bpp colors
        else
            ColorsCount = (int)header.ColorsUsed;

        fixed (byte* ptr = colors)
        {
            var colorsAsSpan = new Span<byte>(ptr, ColorsCount * 4);
            paletteBytes.Slice(0, ColorsCount * 4).CopyTo(colorsAsSpan);
        }
    }

    public int ColorsCount { get; set; }
    public int SizeInBytes => ColorsCount * 4; // 4 bytes per color (RGBA)
    private unsafe fixed byte colors[256 * 4]; // 256 colors, each with 4 bytes (RGBA)

    public unsafe Rgba this[int index]
    {
        get
        {
            if (index < 0 || index >= ColorsCount)
                throw new ArgumentOutOfRangeException(nameof(index));
            return new Rgba(colors[index * 4 + 0], colors[index * 4 + 1], colors[index * 4 + 2], colors[index * 4 + 3]);
        }
    }

    public struct Rgba
    {
        public Rgba(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public byte R;
        public byte G;
        public byte B;
        public byte A;
    }
}

internal struct BitmapInfoHeader
{
    public BitmapInfoHeader(ReadOnlySpan<byte> headerBytes)
    {
        HeaderLength = BinaryPrimitives.ReadInt32LittleEndian(headerBytes.Slice(0, 4));
        if (HeaderLength < 40 || HeaderLength > 124)
            throw new InvalidDataException("Invalid BMP info header length");
        Width = BinaryPrimitives.ReadInt32LittleEndian(headerBytes.Slice(4, 4));
        Height = BinaryPrimitives.ReadInt32LittleEndian(headerBytes.Slice(8, 4));
        Planes = BinaryPrimitives.ReadUInt16LittleEndian(headerBytes.Slice(12, 2));
        BitsPerPixel = BinaryPrimitives.ReadUInt16LittleEndian(headerBytes.Slice(14, 2));
        Compression = (BmpCompression)BinaryPrimitives.ReadInt32LittleEndian(headerBytes.Slice(16, 4));
        SizeImage = BinaryPrimitives.ReadUInt32LittleEndian(headerBytes.Slice(20, 4));
        XPixelsPerMeter = BinaryPrimitives.ReadInt32LittleEndian(headerBytes.Slice(24, 4));
        YPixelsPerMeter = BinaryPrimitives.ReadInt32LittleEndian(headerBytes.Slice(28, 4));
        ColorsUsed = BinaryPrimitives.ReadUInt32LittleEndian(headerBytes.Slice(32, 4));
        ColorsImportant = BinaryPrimitives.ReadUInt32LittleEndian(headerBytes.Slice(36, 4));
    }

    public void WriteTo(Span<byte> buffer)
    {
        if (buffer.Length < 40)
            throw new ArgumentException("Buffer too small for BitmapInfoHeader", nameof(buffer));

        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(0, 4), HeaderLength);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(4, 4), Width);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(8, 4), Height);
        BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(12, 2), Planes);
        BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(14, 2), BitsPerPixel);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(16, 4), (int)Compression);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(20, 4), SizeImage);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(24, 4), XPixelsPerMeter);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(28, 4), YPixelsPerMeter);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(32, 4), ColorsUsed);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(36, 4), ColorsImportant);
    }

    public int HeaderLength { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public ushort Planes { get; set; }
    public ushort BitsPerPixel { get; set; }
    public BmpCompression Compression { get; set; }
    public uint SizeImage { get; set; }
    public int XPixelsPerMeter { get; set; }
    public int YPixelsPerMeter { get; set; }
    public uint ColorsUsed { get; set; }
    public uint ColorsImportant { get; set; }
}

internal enum BmpCompression
{
    None = 0,
    Rle8 = 1,
    Rle4 = 2,
    Bitfields = 3,
    Jpeg = 4,
    Png = 5,
    AlphaBitfields = 6,
    Cmyk = 11,
    CmykRle8 = 12,
    CmykRle4 = 13
}
