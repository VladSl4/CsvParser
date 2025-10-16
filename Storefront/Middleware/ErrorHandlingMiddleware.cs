using Grpc.Core;
using System.Text.Json;

namespace Storefront.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (RpcException ex)
        {
            await HandleRpcExceptionAsync(context, ex);
        }
    }

    private static Task HandleRpcExceptionAsync(HttpContext context, RpcException exception)
    {
        int httpStatusCode = exception.StatusCode switch
        {
            StatusCode.NotFound => StatusCodes.Status404NotFound,
            StatusCode.InvalidArgument => StatusCodes.Status400BadRequest,
            StatusCode.PermissionDenied => StatusCodes.Status403Forbidden,
            StatusCode.Unauthenticated => StatusCodes.Status401Unauthorized,
            StatusCode.AlreadyExists => StatusCodes.Status409Conflict,
            StatusCode.Unavailable => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = httpStatusCode;
        
        var problemDetails = new
        {
            StatusCode = httpStatusCode,
            Detail = exception.Status.Detail,
            Message = exception.Message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}