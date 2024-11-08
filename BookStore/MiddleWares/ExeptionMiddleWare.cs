using Azure.Core.Serialization;
using BookStore.Extentions;
using System.Net;
using System.Text.Json;

namespace BookStore.Middlewares
{
    public class ExeptionMiddleWare 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExeptionMiddleWare> _logger;
        private readonly IHostEnvironment _env;

        public ExeptionMiddleWare(RequestDelegate next, ILogger<ExeptionMiddleWare> logger, IHostEnvironment env) 
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        // InvokeAsync
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , ex.Message);
                // Production => LOG ex in Db
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                var Response = _env.IsDevelopment() ? new ApiExeptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, (string)ex.StackTrace)
                                                    : new ApiExeptionResponse((int)HttpStatusCode.InternalServerError);
                var Options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var JsonResponse = JsonSerializer.Serialize(Response , Options);
                 context.Response.WriteAsJsonAsync(JsonResponse);
            }

        }


    }
}
