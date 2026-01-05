using DIGESA.Models.CannabisModels.Formularios;
using DIGESA.Repositorios.InterfacesCannabis;

namespace DIGESA.Models.CannabisModels.Validadores;

public class ProductoValidator : IValidator<DatosProductoVM>
{
    public List<string> Validate(DatosProductoVM producto)
    {
        var errors = new List<string>();

        if (producto == null)
        {
            errors.Add("Los datos del producto son obligatorios.");
            return errors;
        }

        if (string.IsNullOrWhiteSpace(producto.NombreProducto))
            errors.Add("El nombre del producto es obligatorio.");

        if (producto.ProductoUnidadId == null)
            errors.Add("La unidad del producto es obligatoria.");

        if (!producto.SelectedFormaIds.Any() &&
            string.IsNullOrWhiteSpace(producto.NombreOtraForma))
        {
            errors.Add("Debe indicar la forma farmacéutica.");
        }

        if (!producto.SelectedViaAdmIds.Any() &&
            string.IsNullOrWhiteSpace(producto.NombreOtraViaAdm))
        {
            errors.Add("Debe indicar la vía de administración.");
        }

        if (producto.UsaDosisRescateEnum == EnumViewModel.UsaDosisRescate.Si &&
            string.IsNullOrWhiteSpace(producto.DetDosisRescate))
        {
            errors.Add("Debe detallar la dosis de rescate.");
        }

        return errors;
    }
}