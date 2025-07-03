using DIGESA.Models.Entities.DBDIGESA;

public class SolicitudDiagnosticoRepository : ISolicitudDiagnosticoRepository
{
    private readonly DbContextDigesa _context;

    public SolicitudDiagnosticoRepository(DbContextDigesa context)
    {
        _context = context;
    }

    public async Task AddAsync(SolicitudDiagnostico solicitudDiagnostico)
    {
        await _context.SolicitudDiagnostico.AddAsync(solicitudDiagnostico);
        await _context.SaveChangesAsync();
    }
}