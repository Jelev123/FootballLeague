namespace FootballLeague.ErrorHandler
{
    using Microsoft.AspNetCore.Http;
    using System.Net;
    using System.Threading.Tasks;
    using System;
    using System.Text.Json;
    using Amazon.CloudWatchLogs.Model;
    using FootballLeague.Data.Common.Error;

    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case ArgumentException ae:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case ResourceAlreadyExistsException raee:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case ResourceNotFoundException rnfe:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case NullReferenceException nre:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new ErrorMessage { Message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
