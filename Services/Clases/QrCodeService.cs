using ZXing;
using ZXing.Common;
using ZXing.Rendering;
using System.Drawing;

public class QrCodeService
{
    public byte[] GenerateQrCode(string text, int width = 300, int height = 300)
    {
        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Height = height,
                Width = width,
                Margin = 1
            }
        };

        var pixelData = writer.Write(text);
        return pixelData.Pixels;
    }
}