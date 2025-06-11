using DIGESA.Models.DTOs;

namespace DIGESA.Services.Interfaces;

public interface IPacienteService
{
    Task<RegistrationResult> RegistrarPacienteAsync(PacienteRegistroDTO model);
}