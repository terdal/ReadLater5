using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReadLater5API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ReadLater5API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (HttpListenerException responseException)
            {
                _logger.LogError($"Something went wrong: {responseException}");
                await HandleExceptionAsync(httpContext, responseException);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something went wrong: {exception}");
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string content = exception.Message;

            return context.Response.WriteAsync(new ErrorDetailModel()
            {
                StatusCode = context.Response.StatusCode,
                Message = content
            }.ToString());
        }

        private Task HandleExceptionAsync(HttpContext context, HttpListenerException responseException)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = responseException.ErrorCode;
            string content = responseException.Message;

            return context.Response.WriteAsync(new ErrorDetailModel()
            {
                StatusCode = context.Response.StatusCode,
                Message = content
            }.ToString());
        }
    }
}
