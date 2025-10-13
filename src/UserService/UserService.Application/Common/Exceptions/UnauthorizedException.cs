namespace UserService.Application.Common.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message, string? details = null)
        : base($"{message} {details}")
    {
    }
}