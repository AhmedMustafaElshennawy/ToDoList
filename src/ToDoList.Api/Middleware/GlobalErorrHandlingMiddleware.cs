using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDoList.Api.Middleware
{
    public class GlobalErorrHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErorrHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception exception) 
            {
                await HandleExceptionAsync( httpContext, exception);
                
            }
        }

        public Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // returns ==> 500 if unexpected
            var result = JsonSerializer.Serialize(new { code, messge = "an erorr accurred" });
            httpContext.Response.ContentType = "application/json";
            return httpContext.Response.WriteAsync(result);
        }
    }
}
