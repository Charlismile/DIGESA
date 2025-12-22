namespace DIGESA.Models.CannabisModels.Configuracion
{
    public class EmailSettings
    {
        // =============================
        // Servidor SMTP
        // =============================
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }

        // =============================
        // Credenciales
        // =============================
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;

        // =============================
        // Seguridad
        // =============================
        public bool EnableSsl { get; set; }

        // =============================
        // Remitente
        // =============================
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
    }
}