using System.Text.Json;
using EventsService.Application.Common.Extensions;
using UserService.Application.Common.Exceptions;

namespace UserService.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex)
        {
            await HandleException(context, ex);
        }
        catch (EntityNotFoundException ex)
        {
            await HandleException(context, ex);
        }
        catch (UnauthorizedException ex)
        {
            await HandleException(context, ex);
        }
        catch (EntitiesNotFoundException ex)
        {
            await HandleException(context, ex);
        }
        catch (PhotoNotFoundException ex)
        {
            await HandleException(context, ex);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }

    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        string message = exception.Message;
        int statusCode = exception switch
        {
            BusinessException => 400,
            EntityNotFoundException => 404,
            EntitiesNotFoundException => 405,
            PhotoNotFoundException => 406,
            UnauthorizedException => 401,
            _ => 500
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            StatusCode = statusCode,
            Message = message,
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}