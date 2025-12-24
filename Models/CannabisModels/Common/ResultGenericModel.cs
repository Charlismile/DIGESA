namespace DIGESA.Models.CannabisModels.Common;

public class ResultGenericModel<T> : ResultModel
{
    public T? Data { get; set; }
}