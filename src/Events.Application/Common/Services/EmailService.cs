using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using Events.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace Events.Application.Common.Services;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _senderEmail;
    private readonly string _senderName;

    public EmailService(IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection("EmailSettings");
        _smtpServer = emailSettings["SmtpServer"]!;
        _smtpPort = int.Parse(emailSettings["SmtpPort"]!);
        _smtpUsername = emailSettings["SmtpUsername"]!;
        _smtpPassword = emailSettings["SmtpPassword"]!;
        _senderEmail = emailSettings["SenderEmail"]!;
        _senderName = emailSettings["SenderName"]!;
    }

    public async Task SendEventUpdatedEmailAsync(EventDTO @event, User participant)
    {
        var subject = $"Event Update: {@event.Title}";
        var body = $"Dear {participant.Name},\n\n" +
                  $"The event '{@event.Title}' has been updated.\n" +
                  $"New details:\n" +
                  $"Date: {@event.EventDate}\n" +
                  $"Location: {@event.Location}\n" +
                  $"Description: {@event.Description}\n\n" +
                  "Best regards,\n" +
                  "Events Team";

        await SendEmailAsync(participant.Email, subject, body);
    }

    public async Task SendEventDeletedEmailAsync(string eventTitle, User participant)
    {
        var subject = $"Event Cancelled: {eventTitle}";
        var body = $"Dear {participant.Name},\n\n" +
                  $"We regret to inform you that the event '{eventTitle}' has been cancelled.\n\n" +
                  "Best regards,\n" +
                  "Events Team";

        await SendEmailAsync(participant.Email, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using var client = new SmtpClient(_smtpServer, _smtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_smtpUsername, _smtpPassword)
        };

        using var message = new MailMessage
        {
            From = new MailAddress(_senderEmail, _senderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };
        message.To.Add(toEmail);

        await client.SendMailAsync(message);
    }
} 