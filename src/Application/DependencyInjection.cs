using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            c.AddOpenBehavior(typeof(ValidationBehavior<,>));
            c.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddAutoMapper(typeof(MappingProfiles));
        services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
    }
}
