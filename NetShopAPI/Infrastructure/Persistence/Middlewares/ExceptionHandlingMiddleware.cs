using NetShopAPI.Common;

namespace NetShopAPI.Infrastructure.Persistence.Middlewares
{
    public class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingMiddleware(
            RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {

            _next = next;
            _logger = logger;
            _environment = env;
        }


        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }

            catch (Exception ex)
            {
                var traceId = httpContext.TraceIdentifier;

                _logger.LogError(ex, "Unhandled exception. TraceId={TraceId}", traceId);

                httpContext.Response.ContentType = "application/json";

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var message = _environment.IsDevelopment()
                    ? ex.Message : "Внутренняя ошибка сервера!";

                var error = new ApiError("INTERNAL_SERVER_ERROR", message, traceId);

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }


    }
}
