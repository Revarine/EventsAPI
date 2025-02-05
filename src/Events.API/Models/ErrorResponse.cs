namespace Events.API.Models;
using System.Net;

public class ErrorResponse
{
    public string Type { get; set; }
    public string Message { get; set; }
    public string? Details { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public ErrorResponse(string type, string message, string? details = null)
    {
        Type = type;
        Message = message;
        Details = details;
    }
} 