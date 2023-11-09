namespace QianShiChat.WebApi.Filters
{
    public class GlobalExeptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExeptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await Results.Problem()
                     .ExecuteAsync(context);
        }
    }
}
