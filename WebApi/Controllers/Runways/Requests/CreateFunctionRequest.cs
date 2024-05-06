namespace webapi.Controllers.Runways.Requests;

public class CreateFunctionRequest
{
    public required string? Description { get; set; }
    
    public required CreateFunctionPointRequest[] Points { get; set; }
}