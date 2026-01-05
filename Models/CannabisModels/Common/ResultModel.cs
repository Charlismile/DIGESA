namespace DIGESA.Models.CannabisModels.Common;

public class ResultModel
{
    public bool Success { get; set; }
    
    public List<string> Errors { get; set; } = new List<string>();
    public string Message { get; set; } = string.Empty;
}