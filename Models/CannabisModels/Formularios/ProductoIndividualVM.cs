using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DIGESA.Models.CannabisModels.Common;

namespace DIGESA.Models.CannabisModels.Formularios
{
    public class ProductoIndividualVM
    {
        private EnumViewModel.NombreProductoE _nombreProductoEnum;
        private string? _nombreProducto;
        private string _nombreComercialProd = string.Empty;
        private string? _nombreOtraForma;
        private string? _nombreOtraViaAdm;
        private string? _nombreOtraConcentracion;
        private string? _detDosisPaciente;
        private string? _detDosisRescate;
        
        [Required(ErrorMessage = "Seleccione un principio activo")]
        public EnumViewModel.NombreProductoE NombreProductoEnum
        {
            get => _nombreProductoEnum;
            set
            {
                if (_nombreProductoEnum != value)
                {
                    _nombreProductoEnum = value;
                    // Limpiar el campo "Otro" si no está seleccionado
                    if (value != EnumViewModel.NombreProductoE.OTRO)
                    {
                        NombreProducto = null;
                    }
                }
            }
        }
        
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string? NombreProducto 
        { 
            get => _nombreProducto;
            set => _nombreProducto = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
        }
        
        [Required(ErrorMessage = "El nombre comercial es requerido")]
        [MaxLength(200, ErrorMessage = "El nombre comercial no puede exceder 200 caracteres")]
        public string NombreComercialProd 
        { 
            get => _nombreComercialProd;
            set => _nombreComercialProd = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
        }

        [Required(ErrorMessage = "Seleccione al menos una forma farmacéutica")]
        public List<int> SelectedFormaIds { get; set; } = new();
        
        public string? NombreOtraForma 
        { 
            get => _nombreOtraForma;
            set => _nombreOtraForma = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
        }
        public bool IsOtraFormaSelected { get; set; }

        public string? ProductoUnidad { get; set; }

        [Required(ErrorMessage = "Seleccione al menos una vía de administración")]
        public List<int> SelectedViaAdmIds { get; set; } = new();
        
        public string? NombreOtraViaAdm 
        { 
            get => _nombreOtraViaAdm;
            set => _nombreOtraViaAdm = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
        }
        public bool IsOtraViaAdmSelected { get; set; }

        [Required(ErrorMessage = "Seleccione una unidad")]
        public int? ProductoUnidadId { get; set; }

        [Required(ErrorMessage = "La concentración es requerida")]
        [Range(0.01, 10000, ErrorMessage = "La concentración debe ser mayor a 0")]
        public decimal? CantidadConcentracion { get; set; }
        
        [Required(ErrorMessage = "Seleccione al menos una concentración")]
        public List<EnumViewModel.ConcentracionE> ConcentracionesSeleccionadas { get; set; } = new();
        
        public bool IsOtraConcentracionSelected { get; set; }
        public string? NombreOtraConcentracion 
        { 
            get => _nombreOtraConcentracion;
            set => _nombreOtraConcentracion = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
        }

        [Required(ErrorMessage = "La dosis es requerida")]
        [MaxLength(500, ErrorMessage = "Máximo 500 caracteres")]
        public string? DetDosisPaciente 
        { 
            get => _detDosisPaciente;
            set => _detDosisPaciente = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
        }
        
        [Required(ErrorMessage = "La frecuencia es requerida")]
        [Range(1, 6, ErrorMessage = "La frecuencia debe ser entre 1 y 6 veces al día")]
        public int? DosisFrecuencia { get; set; }
        
        [Required(ErrorMessage = "La duración es requerida")]
        [Range(1, 31, ErrorMessage = "La duración debe ser entre 1 y 31 días")]
        public int? DosisDuracion { get; set; }

        [Required(ErrorMessage = "Seleccione si usa dosis de rescate")]
        public EnumViewModel.UsaDosisRescate UsaDosisRescateEnum { get; set; }
        
        [MaxLength(500, ErrorMessage = "Máximo 500 caracteres")]
        public string? DetDosisRescate 
        { 
            get => _detDosisRescate;
            set => _detDosisRescate = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
        }

        // Método auxiliar para normalización
        private string NormalizarTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            texto = texto.Trim();
            
            // Convertir a mayúsculas
            texto = texto.ToUpperInvariant();
            
            // Remover múltiples espacios
            while (texto.Contains("  "))
                texto = texto.Replace("  ", " ");
                
            return texto;
        }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NombreProductoEnum == EnumViewModel.NombreProductoE.OTRO && 
                string.IsNullOrWhiteSpace(NombreProducto))
            {
                yield return new ValidationResult(
                    "Especifique el nombre del producto cuando selecciona 'Otro'",
                    new[] { nameof(NombreProducto) });
            }
        }
    }
}