using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Routing;

public class MyRegisterRequest
{
    public required string Email { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }

}