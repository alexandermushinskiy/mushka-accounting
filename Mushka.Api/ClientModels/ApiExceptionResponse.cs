namespace Mushka.Api.ClientModels;

public class ApiExceptionResponse
{
    public ApiExceptionResponse(string message)
    {
        Message = message;
    }

    public ApiExceptionResponse(string message, string details)
    {
        Message = message;
        Details = details;
    }

    public string Message { get; }

    public string Details { get; }
}
