using QRCoder;

public interface IQRService
{
    string GenerateQRCode(string text);
}

public class QRService : IQRService
{
    public string GenerateQRCode(string text)
    {
        using var qrGenerator = new QRCoder.QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(text, QRCoder.QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrBytes = qrCode.GetGraphic(20);

        return $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}";
    }
}