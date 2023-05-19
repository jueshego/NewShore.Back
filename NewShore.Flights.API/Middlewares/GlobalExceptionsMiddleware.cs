using System.Net;

namespace NewShore.Flights.API.Middlewares
{
    public class GlobalExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;

        public GlobalExceptionsMiddleware(RequestDelegate next, Serilog.ILogger loger)
        {
            _next = next;
            _logger = loger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ha ocurrido un evento no controlado: {ex.Message} \n en: {ex.StackTrace}");
                HandleError(httpContext, ex);
            }
        }

        private void HandleError(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";

            switch (ex)
            {
                //case BussinessException e
                //    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //    break;

                //case KeyNotFoundException e
                //    httpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;

                default:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
        }
    }
}
