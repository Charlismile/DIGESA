namespace DIGESA.Services.Clases;
using QRCoder;
using System.IO;

public class QRCodeService
{
    public byte[] GenerarCodigoQR(string contenido)
    {
        using var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }
}
