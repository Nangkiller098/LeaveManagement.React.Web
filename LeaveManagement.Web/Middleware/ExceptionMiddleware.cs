using System.Net;
using LeaveManagement.Application.Exceptions;
using Newtonsoft.Json;



namespace LeaveManagement.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
            
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await  _next(context);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Something Went Wrong in the {context.Request.Path}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var errorDetails = new ErrorDetails
            {
                ErrorType ="Faillure",
                ErrorMessage = ex.Message
            };
            switch (ex)
            {
                case    NotFoundException notFoundException:
                        statusCode= HttpStatusCode.NotFound;
                        errorDetails.ErrorType = "Not Found";
                        break ;
                        default:
                        break;
            }
            string response= JsonConvert.SerializeObject(errorDetails);
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(response);
        }
    }
}
public class ErrorDetails
{
    public string ErrorType { get; set;}
    public string ErrorMessage { get; set;}
}