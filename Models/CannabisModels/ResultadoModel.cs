namespace DIGESA.Models.CannabisModels;

public class ResultModel
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();
    
    public static ResultModel SuccessResult(string message = "Operación exitosa")
    {
        return new ResultModel { Success = true, Message = message };
    }
    
    public static ResultModel ErrorResult(string message, List<string>? errors = null)
    {
        return new ResultModel 
        { 
            Success = false, 
            Message = message, 
            Errors = errors ?? new List<string>() 
        };
    }
}

public class ResultModel<T> : ResultModel
{
    public T? Data { get; set; }
    
    public static ResultModel<T> SuccessResult(T data, string message = "Operación exitosa")
    {
        return new ResultModel<T> 
        { 
            Success = true, 
            Message = message, 
            Data = data 
        };
    }
    
    public new static ResultModel<T> ErrorResult(string message, List<string>? errors = null)
    {
        return new ResultModel<T> 
        { 
            Success = false, 
            Message = message, 
            Errors = errors ?? new List<string>() 
        };
    }
}

public class PaginationModel
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

public class ItemListModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}