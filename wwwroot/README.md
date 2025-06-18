# 🌿 Plataforma DIGESA - Registro de Pacientes de Cannabis Medicinal

> Sistema digital para el registro, revisión y certificación de pacientes panameños o extranjeros que requieran el uso medicinal del cannabis bajo prescripción médica.

Este proyecto tiene como objetivo facilitar el proceso de registro y certificación de pacientes usuarios de cannabis medicinal en Panamá. La plataforma permite generar un carnet con código QR que autoriza la compra legal del producto en farmacias autorizadas.

---

## 🧩 Tecnologías Utilizadas

| Capa | Tecnología |
|------|------------|
| **Frontend** | Blazor Server App + MudBlazor |
| **Backend** | ASP.NET Core 8.0 |
| **Autenticación** | ASP.NET Identity |
| **Base de datos** | SQL Server |
| **Servicios adicionales** | PDFSharp (PDF), QRCoder (QR), SMTP (correo) |
| **Auditoría** | Tabla de auditoría integrada (`AuditoriaAccion`) |
| **Seguridad** | Roles y políticas de acceso (Administrador, Médico, Paciente) |

---

## 📋 Características Principales

- ✅ Registro de pacientes con formulario completo según documento oficial.
- ✅ Gestión de acompañantes autorizados (menores de edad o pacientes con discapacidad).
- ✅ Validación de diagnósticos médicos (CIE-10 y personalizados).
- ✅ Generación automática de tratamiento basado en receta médica.
- ✅ Revisión por parte de funcionarios del Ministerio de Salud.
- ✅ Generación de carnet digital con código QR.
- ✅ Notificaciones por correo electrónico al paciente.
- ✅ Auditoría completa de acciones importantes.

---

## 🗂️ Estructura del Proyecto
DigesaSolution.sln
├── Digesa.Web # Frontend: Blazor Server con interfaz de usuario
├── Digesa.Application # Servicios de negocio e interfaces
├── Digesa.Domain # Entidades y DTOs
├── Digesa.Infrastructure # Contexto EF, repositorios y configuraciones
├── .gitignore
├── README.md
└── appsettings.json # Configuración del entorno

---

## 📦 Base de Datos

La estructura de base de datos está organizada en catálogos y entidades clave:

### Catálogos
- UnidadMedida
- FormaFarmaceutica
- ViaAdministracion

### Entidades principales
- Paciente
- Acompanante
- Medico
- Diagnostico
- Solicitud
- Tratamiento
- Certificacion
- DocumentoAdjunto
- AuditoriaAccion

---

## 🚀 Cómo ejecutar el proyecto

1. **Clona el repositorio:**
   ```bash
   git clone https://github.com/Charlismile/DIGESA.git 
   cd DIGESA
Restaura las dependencias:
bash


1
dotnet restore
Configura la cadena de conexión en appsettings.json:
json


1
2
3
⌄
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=DBDIGESA;Trusted_Connection=True;TrustServerCertificate=True;"
}
Ejecuta la aplicación:
bash


1
dotnet run
Accede desde el navegador:


1
http://localhost:5000
🛡️ Autenticación y Autorización
Se usa ASP.NET Identity para gestión de usuarios y roles.
Roles definidos:
Paciente : puede registrar y ver su solicitud.
Médico : puede revisar y validar solicitudes.
Administrador : gestiona usuarios, aprueba/rechaza solicitudes y genera carnets.
📬 Notificaciones por Correo
El sistema envía correos electrónicos automatizados:

Confirmación de recepción de solicitud.
Notificación de aprobación o rechazo.
Enlace para descargar el carnet digital.
📊 Auditoría
Todas las acciones críticas son registradas en la tabla AuditoriaAccion, incluyendo:

Inicio de sesión
Revisión médica
Aprobación/rechazo de solicitud
Generación de carnet
🧪 Pruebas
Se usan pruebas unitarias para validar lógica crítica.
Se recomienda cubrir servicios como:
SolicitudService
PacienteService
CarnetService
AuthService
🔄 Integración Continua (Opcional)
Se puede agregar una acción básica de GitHub Actions para construir automáticamente el proyecto en cada push.

🤝 Contribuciones
Si deseas contribuir al proyecto, sigue estos pasos:

Haz fork del repositorio.
Crea una nueva rama (git checkout -b feature/nueva-funcionalidad).
Realiza tus cambios y haz commit (git commit -m "Agrega nueva funcionalidad").
Sube los cambios (git push origin feature/nueva-funcionalidad).
Abre un Pull Request describiendo tus cambios.
📞 Contacto
👤 Carlos Ibarra
📧 charlismile@example.com
🌐 GitHub

📄 Licencia
Este proyecto está bajo la licencia MIT. Ver el archivo LICENSE para más detalles.
