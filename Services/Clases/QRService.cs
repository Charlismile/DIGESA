using QRCoder;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using DIGESA.Services.Interfaces;

namespace DIGESA.Services
{
    public class QRService : IQRService
    {
        public string GenerateQRCode(string text)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrBytes = qrCode.GetGraphic(20);

            return $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}";
        }
    }
}