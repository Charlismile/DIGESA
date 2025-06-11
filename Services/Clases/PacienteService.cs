using Microsoft.EntityFrameworkCore;
using DIGESA.Models.DTOs;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Services.Interfaces;

public class PacienteService : IPacienteService
{
    private readonly DbContextDigesa _context;

    public PacienteService(DbContextDigesa context)
    {
        _context = context;
    }

    public async Task<RegistrationResult> RegistrarPacienteAsync(PacienteRegistroDTO model)
    {
        try
        {
            var paciente = new Paciente
            {
                NombreCompleto = model.NombreCompleto,
                TipoDocumento = model.TipoDocumento,
                NumeroDocumento = model.NumeroDocumento,
                Nacionalidad = model.Nacionalidad,
                FechaNacimiento = model.FechaNacimiento,
                Sexo = model.Sexo,
                DireccionResidencia = model.DireccionResidencia,
                TelefonoResidencial = model.TelefonoResidencial,
                TelefonoPersonal = model.TelefonoPersonal,
                TelefonoLaboral = model.TelefonoLaboral,
                CorreoElectronico = model.CorreoElectronico,
                InstalacionSalud = model.InstalacionSalud,
                RegionSalud = model.RegionSalud,
                RequiereAcompanante = model.RequiereAcompanante,
                MotivoRequerimientoAcompanante = model.MotivoRequerimientoAcompanante,
                TipoDiscapacidad = model.TipoDiscapacidad
            };

            await _context.Pacientes.AddAsync(paciente);
            int rowsAffected = await _context.SaveChangesAsync();

            if (model.RequiereAcompanante && model.Acompanante.NombreCompleto != "")
            {
                var acompanante = new Acompanante
                {
                    PacienteId = paciente.Id,
                    NombreCompleto = model.Acompanante.NombreCompleto,
                    TipoDocumento = model.Acompanante.TipoDocumento,
                    NumeroDocumento = model.Acompanante.NumeroDocumento,
                    Nacionalidad = model.Acompanante.Nacionalidad,
                    Parentesco = model.Acompanante.Parentesco
                };

                await _context.Acompanantes.AddAsync(acompanante);
                await _context.SaveChangesAsync();
            }

            return new RegistrationResult { Success = rowsAffected > 0 };
        }
        catch (Exception ex)
        {
            return new RegistrationResult { Success = false, ErrorMessage = ex.Message };
        }
    }
}

public class RegistrationResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}