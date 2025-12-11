namespace DIGESA.Models.CannabisModels;

public class ResultModel
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    
    public static ResultModel Ok(string message = "Operación exitosa")
    {
        return new ResultModel { Success = true, Message = message };
    }
    
    public static ResultModel Error(string message, List<string> errors = null)
    {
        return new ResultModel 
        { 
            Success = false, 
            Message = message, 
            Errors = errors ?? new List<string>() 
        };
    }
}

public class ResultGenericModel<T> : ResultModel
{
    public T Data { get; set; }
    
    public static ResultGenericModel<T> Ok(T data, string message = "Operación exitosa")
    {
        return new ResultGenericModel<T> 
        { 
            Success = true, 
            Message = message, 
            Data = data 
        };
    }
}