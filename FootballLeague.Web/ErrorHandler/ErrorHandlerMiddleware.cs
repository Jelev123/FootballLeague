namespace FootballLeague.ErrorHandler
{
    using Microsoft.AspNetCore.Http;
    using System.Net;
    using System.Threading.Tasks;
    using System;
    using System.Text.Json;

    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)GetStatusCode(exception);

            var errorResponse = new ErrorResponse { Message = exception.Message };

            var json = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(json);
        }

        public static HttpStatusCode GetStatusCode(Exception exception)
        {
            switch (exception)
            {
                case ArgumentException _:
                    return HttpStatusCode.Conflict;
                case ResourceAlreadyExistsException _:
                    return HttpStatusCode.Conflict;
                case ResourceNotFoundException _:
                    return HttpStatusCode.NotFound;
                case NullReferenceException _:
                    return HttpStatusCode.BadRequest;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
    }

    public class ResourceAlreadyExistsException : Exception
    {
        public ResourceAlreadyExistsException(string message) : base(message)
        {
        }
    }

    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message)
        {
        }
    }
}
