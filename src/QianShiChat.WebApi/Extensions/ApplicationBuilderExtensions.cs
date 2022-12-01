namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// application builder extensions.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Use open api
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.EnablePersistAuthorization();
        });
        return app;
    }
}