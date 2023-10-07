
//middleware for error handling//RequestDeligate essential, rest two optional;
//Request Deligate --> what is the next middlware it should go it after its part is done
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
            
        }

        public async Task InvokeAsync(HttpContext context){
            try{
                await _next(context);
            }
            catch (Exception ex){
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json"; //return something to client//need to specify this as we are not in APi controller
                context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) 
                    : new ApiException(context.Response.StatusCode, ex.Message,"Internal Server Error");

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(json);

                //done by default in API controllers   
                // we did this so excpetion isnt handled anywhere else in the app
            }
        }
    }
}