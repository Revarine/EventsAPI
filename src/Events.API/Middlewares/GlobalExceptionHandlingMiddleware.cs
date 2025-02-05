using System.Net;
using Events.API.Models;
using Events.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred");
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = exception switch
        {
            NotFoundException notFoundException => new ErrorResponse(
                "NotFound",
                notFoundException.Message)
            {
                StatusCode = HttpStatusCode.NotFound
            },

            ValidationException validationException => new ErrorResponse(
                "ValidationError",
                validationException.Message)
            {
                StatusCode = HttpStatusCode.BadRequest
            },

            AlreadyExistsException alreadyExistsException => new ErrorResponse(
                "AlreadyExists",
                alreadyExistsException.Message)
            {
                StatusCode = HttpStatusCode.Conflict
            },

            _ => new ErrorResponse(
                "InternalServerError",
                "An internal server error occurred.")
            {
                StatusCode = HttpStatusCode.InternalServerError
            }
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
} 