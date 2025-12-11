using DIGESA.Repositorios.Interfaces;

namespace DIGESA.Repositorios.Services;

public class UtilitiesServices : IUtilities
{
    public string FormatDate(DateTime? date)
    {
        if (date == null)
            return "";

        int day = date.Value.Day;
        int month = date.Value.Month;
        int year = date.Value.Year;

        string monthName = "";
        switch (month)
        {
            case 1:
                monthName = "enero";
                break;
            case 2:
                monthName = "febrero";
                break;
            case 3:
                monthName = "marzo";
                break;
            case 4:
                monthName = "abril";
                break;
            case 5:
                monthName = "mayo";
                break;
            case 6:
                monthName = "junio";
                break;
            case 7:
                monthName = "julio";
                break;
            case 8:
                monthName = "agosto";
                break;
            case 9:
                monthName = "septiembre";
                break;
            case 10:
                monthName = "octubre";
                break;
            case 11:
                monthName = "noviembre";
                break;
            case 12:
                monthName = "diciembre";
                break;
        }

        return day.ToString().PadLeft(2, '0') + "-" + monthName + "-" + year.ToString();
    }
}
    