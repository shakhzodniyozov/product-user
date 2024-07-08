using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.ExceptionHandled = context switch
        {
            { Exception: ValidationException } => HandleValidationException(context),
            _ => HandleUnknownException(context)
        };
        base.OnException(context);
    }

    public bool HandleUnknownException(ExceptionContext context)
    {
        ProblemDetails details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Detail = context.Exception.Message,
        };

        context.Result = new ObjectResult(details);

        return true;
    }

    public bool HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;
        ProblemDetails details = new ValidationProblemDetails(exception.Errors)
        {
            Title = "Bad Request.",
            Detail = "One or more validation errors was occured",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        context.Result = new BadRequestObjectResult(details);

        return true;
    }
}
