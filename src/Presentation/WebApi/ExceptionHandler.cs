
using Application;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext httpContext, Exception ex)
    {
        switch(ex)
        {
            case ValidationException _ :
                await HandleValidationException(httpContext, (ValidationException)ex);
            break;
        }
    }

    private async Task HandleValidationException(HttpContext httpContext, ValidationException ex)
    {
        ProblemDetails pd = new ValidationProblemDetails(ex.Errors)
        {
            Title = "Bad Request.",
            Detail = "One or more validation errors was occured",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        httpContext.Response.StatusCode = 400;
        await httpContext.Response.WriteAsJsonAsync(pd);
    }
}
