using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbNombreProductoPaciente
{
    public int Id { get; set; }

    public string? NombreProducto { get; set; }

    public int? PacienteId { get; set; }

    public string? FormaFarmaceutica { get; set; }

    public decimal? CantidadConcentracion { get; set; }

    public string? NombreConcentracion { get; set; }

    public string? ViaConsumoProducto { get; set; }

    public string? DetDosisPaciente { get; set; }

    public bool UsaDosisRescate { get; set; }

    public string? ProductoUnidad { get; set; }

    public int? DosisDuracion { get; set; }

    public int? DosisFrecuencia { get; set; }

    public string? NombreComercialProd { get; set; }

    public string? DetDosisRescate { get; set; }

    public int? ProductoUnidadId { get; set; }

    public virtual TbPaciente? Paciente { get; set; }

    public virtual TbUnidades? ProductoUnidadNavigation { get; set; }
}
