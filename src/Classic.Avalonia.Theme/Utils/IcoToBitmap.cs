using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Classic.Avalonia.Theme.Utils;

internal class IcoToBitmap
{
    public static bool TryExtractBitmapFromIcon(in IcoIcon.IconEntry iconEntry, ReadOnlySpan<byte> dibData, out Bitmap bitmap)
    {
        bitmap = null!;
        var dibHeader = new BitmapInfoHeader(dibData);
        var palette = new BitmapPalette(dibHeader, dibData.Slice(dibHeader.HeaderLength));

        if (dibHeader.Compression != BmpCompression.None)
        {
            return false;
        }

        if (dibHeader.Width != iconEntry.ActualWidth ||
            dibHeader.Height != iconEntry.ActualHeight * 2 /* dib contains 1-bitmap and 1-mask */)
        {
            return false;
        }

        int andMaskSize = (iconEntry.ActualWidth + 31) / 32 * 4 * iconEntry.ActualHeight;
        var pixelData = dibData.Slice(dibHeader.HeaderLength + palette.SizeInBytes, dibData.Length - dibHeader.HeaderLength - andMaskSize - palette.SizeInBytes);
        var andMask = dibData.Slice(dibData.Length - andMaskSize, andMaskSize);

        var writeableBitmap = new WriteableBitmap(new PixelSize(iconEntry.ActualWidth, iconEntry.ActualHeight),
            new Vector(96, 96), PixelFormat.Rgba8888, AlphaFormat.Unpremul);

        using var locked = writeableBitmap.Lock();
        unsafe
        {
            var pixels = new Span<byte>((byte*)locked.Address, locked.Size.Width * locked.Size.Height * 4);

            int wstep = iconEntry.BitsPerPixel == 4 ? 2 : 1;

            // Unified loop
            for (int h = 0; h < locked.Size.Height; h++)
            {
                for (int w = 0; w < locked.Size.Width; w += wstep)
                {
                    if (iconEntry.BitsPerPixel == 32)
                    {
                        var pixelDataLine = pixelData.Slice(h * locked.Size.Width * 4, locked.Size.Width * 4);
                        int index = ((locked.Size.Height - 1 - h) * locked.Size.Width + w) * 4;
                        pixels[index + 2] = pixelDataLine[w * 4 + 0];
                        pixels[index + 1] = pixelDataLine[w * 4 + 1];
                        pixels[index + 0] = pixelDataLine[w * 4 + 2];
                        pixels[index + 3] = pixelDataLine[w * 4 + 3];
                    }
                    else if (iconEntry.BitsPerPixel == 24)
                    {
                        var pixelDataLine = pixelData.Slice(h * locked.Size.Width * 3, locked.Size.Width * 3);
                        var andMaskLine = andMask.Slice(h * ((iconEntry.ActualWidth + 31) / 32) * 4, (iconEntry.ActualWidth + 31) / 32 * 4);
                        int index = ((iconEntry.ActualHeight - 1 - h) * locked.Size.Width + w) * 4;
                        int andIndex = w / 8;
                        byte andMaskByte = andMaskLine[andIndex];
                        pixels[index + 2] = pixelDataLine[w * 3 + 0];
                        pixels[index + 1] = pixelDataLine[w * 3 + 1];
                        pixels[index + 0] = pixelDataLine[w * 3 + 2];
                        pixels[index + 3] = ((andMaskByte & (1 << (7 - (w % 8)))) == 0) ? (byte)255 : (byte)0;
                    }
                    else if (iconEntry.BitsPerPixel == 8)
                    {
                        var pixelDataLine = pixelData.Slice(h * locked.Size.Width, locked.Size.Width);
                        var andMaskLine = andMask.Slice(h * ((iconEntry.ActualWidth + 31) / 32) * 4, (iconEntry.ActualWidth + 31) / 32 * 4);
                        int index = ((iconEntry.ActualHeight - 1 - h) * locked.Size.Width + w) * 4;
                        int andIndex = w / 8;
                        byte andMaskByte = andMaskLine[andIndex];
                        var color = palette[pixelDataLine[w]];
                        pixels[index + 0] = color.R;
                        pixels[index + 1] = color.G;
                        pixels[index + 2] = color.B;
                        pixels[index + 3] = ((andMaskByte & (1 << (7 - (w % 8)))) == 0) ? (byte)255 : (byte)0;
                    }
                    else if (iconEntry.BitsPerPixel == 4)
                    {
                        var pixelDataLine = pixelData.Slice(h * locked.Size.Width / 2, locked.Size.Width / 2);
                        var andMaskLine = andMask.Slice(h * ((iconEntry.ActualWidth + 31) / 32) * 4, (iconEntry.ActualWidth + 31) / 32 * 4);
                        int index = ((iconEntry.ActualHeight - 1 - h) * locked.Size.Width + w) * 4;
                        int andIndex = w / 8;
                        byte andMaskByte = andMaskLine[andIndex];
                        var color1 = palette[pixelDataLine[w / 2] & 0x0F];
                        pixels[index + 0] = color1.R;
                        pixels[index + 1] = color1.G;
                        pixels[index + 2] = color1.B;
                        pixels[index + 3] = ((andMaskByte & (1 << (7 - (w % 8)))) == 0) ? (byte)255 : (byte)0;

                        if (w + 1 < locked.Size.Width)
                        {
                            var color2 = palette[pixelDataLine[w / 2] >> 4];
                            pixels[index + 4 + 0] = color2.R;
                            pixels[index + 4 + 1] = color2.G;
                            pixels[index + 4 + 2] = color2.B;
                            pixels[index + 4 + 3] = ((andMaskByte & (1 << (7 - ((w + 1) % 8)))) == 0) ? (byte)255 : (byte)0;
                        }
                    }
                    else
                    {
                        return false; // Unsupported bits per pixel
                    }
                }
            }
        }

        bitmap = writeableBitmap;
        return true;
    }
}

