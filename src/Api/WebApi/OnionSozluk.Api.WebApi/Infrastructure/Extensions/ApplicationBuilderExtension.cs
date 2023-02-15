﻿using Microsoft.AspNetCore.Diagnostics;
using OnionSozluk.Common.Infrastructure.Exceptions;
using OnionSozluk.Common.Infrastructure.Results;
using System.Net;

namespace OnionSozluk.Api.WebApi.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtension
    {
        // middleware
        public static IApplicationBuilder ConfigureExceptionHandling(this IApplicationBuilder app, //middleware buidler
          bool includeExceptionDetails = false, // hata ayrıntılarını göster
          bool useDefaultHandlingResponse = true, // default hata yakalayıcı gösteri
          Func<HttpContext, Exception, Task> handleException = null) //istersen hataları yakalamak için func. girebilirsin.
        {

            app.UseExceptionHandler(options =>
            {
                options.Run(context =>
                {
                    var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();

                    if (!useDefaultHandlingResponse && handleException == null)
                        throw new ArgumentException(nameof(handleException),
                            $"{nameof(handleException)} cannot be null when {nameof(useDefaultHandlingResponse)} is false.");

                    if (!useDefaultHandlingResponse && handleException != null)
                        return handleException(context, exceptionObject.Error);

                    return DefaultHandleException(context, exceptionObject.Error, includeExceptionDetails);
                });
            });

            return app;
        }

        private static async Task DefaultHandleException(HttpContext context, Exception exception, bool includeExceptionDetails)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Internal server error occured!";

            if (exception is UnauthorizedAccessException)
                statusCode = HttpStatusCode.Unauthorized;

            if (exception is DatabaseValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                var validationResponse = new ValidationResponseModel(exception.Message);
                await WriteResponse(context, statusCode, validationResponse);
                return;
            }

            var res = new
            {
                HttpStatusCode = (int)statusCode,
                Detail = includeExceptionDetails ? exception.ToString() : message
            };

            await WriteResponse(context, statusCode, res);
        }

        private static async Task WriteResponse(HttpContext context, HttpStatusCode statusCode, object responseObj)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsJsonAsync(responseObj);
        }
    }
}
