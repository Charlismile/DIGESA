using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DIGESA.Models.CannabisModels.Common;

namespace DIGESA.Models.CannabisModels.Formularios
{
    public class ProductoIndividualVM
    {
        [Required(ErrorMessage = "El principio activo es requerido")]
        public string NombreProducto { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Seleccione un tipo de producto")]
        public EnumViewModel.NombreProductoE NombreProductoEnum { get; set; }

        [Required(ErrorMessage = "El nombre comercial es requerido")]
        public string? NombreComercialProd { get; set; }

        [Required(ErrorMessage = "Seleccione al menos una forma farmacéutica")]
        public List<int> SelectedFormaIds { get; set; } = new();
        
        public string? NombreOtraForma { get; set; }
        public bool IsOtraFormaSelected { get; set; }

        public string? ProductoUnidad { get; set; }

        [Required(ErrorMessage = "Seleccione al menos una vía de administración")]
        public List<int> SelectedViaAdmIds { get; set; } = new();
        
        public string? NombreOtraViaAdm { get; set; }
        public bool IsOtraViaAdmSelected { get; set; }

        [Required(ErrorMessage = "Seleccione una unidad")]
        public int? ProductoUnidadId { get; set; }

        [Required(ErrorMessage = "La concentración es requerida")]
        [Range(0.01, 10000, ErrorMessage = "La concentración debe ser mayor a 0")]
        public decimal? CantidadConcentracion { get; set; }
        
        [Required(ErrorMessage = "Seleccione al menos una concentración")]
        public List<EnumViewModel.ConcentracionE> ConcentracionesSeleccionadas { get; set; } = new();
        
        public bool IsOtraConcentracionSelected { get; set; }
        public string? NombreOtraConcentracion { get; set; }

        [Required(ErrorMessage = "La dosis es requerida")]
        public string? DetDosisPaciente { get; set; }
        
        [Required(ErrorMessage = "La frecuencia es requerida")]
        [Range(1, 6, ErrorMessage = "La frecuencia debe ser entre 1 y 6 veces al día")]
        public int? DosisFrecuencia { get; set; }
        
        [Required(ErrorMessage = "La duración es requerida")]
        [Range(1, 31, ErrorMessage = "La duración debe ser entre 1 y 31 días")]
        public int? DosisDuracion { get; set; }

        [Required(ErrorMessage = "Seleccione si usa dosis de rescate")]
        public EnumViewModel.UsaDosisRescate UsaDosisRescateEnum { get; set; }
        
        public string? DetDosisRescate { get; set; }
    }
}