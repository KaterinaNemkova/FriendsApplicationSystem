namespace UserService.Application.Common.Exceptions;

public class BusinessException : Exception
{
    public int StatusCode { get; set; } = 400;

    public BusinessException(string message) : base(message)
    {
    }

    public BusinessException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public BusinessException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public BusinessException(string message, int statusCode, Exception innerException) 
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}