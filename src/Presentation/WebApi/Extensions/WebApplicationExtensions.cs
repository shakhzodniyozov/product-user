namespace WebApi;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.MapUserEndpoints();
        app.MapProductsEndpoints();
        
        return app;
    }
}
