using SendGrid;
using SendGrid.Helpers.Mail;

namespace Hospital.AppointmentService.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IConfiguration config,
        ILogger<EmailService> logger
        )
    { 
        _config = config; 
        _logger = logger; 
    }

    public async Task SendAppointmentConfirmationAsync(
        string patientEmail, string patientName,
        string doctorName, string doctorEmail,
        DateTime appointmentDate, string? notes)
    {
        // Patient notification
        await SendEmailAsync(
            to: patientEmail, toName: patientName,
            subject: $"Appointment Confirmed — {appointmentDate:dddd, dd MMM yyyy HH:mm}",
            html: BuildPatientEmail(patientName, doctorName, appointmentDate, notes));

        // Doctor notification
        await SendEmailAsync(
            to: doctorEmail, toName: $"Dr. {doctorName}",
            subject: $"New Appointment — Patient {patientName} on {appointmentDate:dd MMM yyyy HH:mm}",
            html: BuildDoctorEmail(patientName, doctorName, appointmentDate, notes));
    }

    private async Task SendEmailAsync(string to, string toName, string subject, string html)
    {
        var apiKey = _config["SendGrid:ApiKey"];
        var fromEmail = _config["SendGrid:FromEmail"] ?? "noreply@hospital.com";
        var fromName = _config["SendGrid:FromName"] ?? "Hospital Management";

        if (string.IsNullOrEmpty(apiKey) || apiKey == "USE_PAPERCUT_LOCALLY")
        {
            // Local mode: send via SMTP to PaperCut
            await SendViaSmtpAsync(to, toName, subject, html, fromEmail, fromName);
            return;
        }

        // Production: use SendGrid API
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(fromEmail, fromName);
        var toAddr = new EmailAddress(to, toName);
        var msg = MailHelper.CreateSingleEmail(from, toAddr, subject, null, html);
        var response = await client.SendEmailAsync(msg);

        if ((int)response.StatusCode >= 400)
            _logger.LogError("SendGrid error {StatusCode} sending to {Email}",
                response.StatusCode, to);
        else
            _logger.LogInformation("Email sent to {Email}: {Subject}", to, subject);
    }

    private async Task SendViaSmtpAsync(string to, string toName,
        string subject, string html,
        string fromEmail, string fromName)
    {
        // PaperCut SMTP: localhost:25 by default
        var smtpHost = _config["Email:SmtpHost"] ?? "localhost";
        var smtpPort = int.Parse(_config["Email:SmtpPort"] ?? "25");

        using var client = new System.Net.Mail.SmtpClient(smtpHost, smtpPort)
        {
            EnableSsl = false,
            DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
        };

        using var message = new System.Net.Mail.MailMessage
        {
            From = new System.Net.Mail.MailAddress(fromEmail, fromName),
            Subject = subject,
            Body = html,
            IsBodyHtml = true
        };

        message.To.Add(new System.Net.Mail.MailAddress(to, toName));
        await client.SendMailAsync(message);
        _logger.LogInformation("[PaperCut] Email sent to {Email}: {Subject}", to, subject);
    }

    private static string BuildPatientEmail(string patient, string doctor,
        DateTime date, string? notes) => $"""
        <html><body style='font-family:Segoe UI,Arial;color:#333'>
          <h2 style='color:#1F4E79'>Appointment Confirmed</h2>
          <p>Dear <strong>{patient}</strong>,</p>
          <p>Your appointment has been confirmed:</p>
          <table style='border-collapse:collapse;width:100%'>
            <tr><td style='padding:8px;border:1px solid #ddd'><b>Doctor</b></td>
                <td style='padding:8px;border:1px solid #ddd'>Dr. {doctor}</td></tr>
            <tr><td style='padding:8px;border:1px solid #ddd'><b>Date &amp; Time</b></td>
                <td style='padding:8px;border:1px solid #ddd'>{date:dddd, dd MMMM yyyy 'at' HH:mm}</td></tr>
            {(notes != null ? $"<tr><td style='padding:8px;border:1px solid #ddd'><b>Notes</b></td><td style='padding:8px;border:1px solid #ddd'>{notes}</td></tr>" : "")}
          </table>
          <p style='color:#666;font-size:12px'>Hospital Management System</p>
        </body></html>
        """;

    private static string BuildDoctorEmail(string patient, string doctor,
        DateTime date, string? notes) => $"""
        <html><body style='font-family:Segoe UI,Arial;color:#333'>
          <h2 style='color:#1F4E79'>New Patient Appointment</h2>
          <p>Dear <strong>Dr. {doctor}</strong>,</p>
          <p>A new appointment has been scheduled:</p>
          <table style='border-collapse:collapse;width:100%'>
            <tr><td style='padding:8px;border:1px solid #ddd'><b>Patient</b></td>
                <td style='padding:8px;border:1px solid #ddd'>{patient}</td></tr>
            <tr><td style='padding:8px;border:1px solid #ddd'><b>Date &amp; Time</b></td>
                <td style='padding:8px;border:1px solid #ddd'>{date:dddd, dd MMMM yyyy 'at' HH:mm}</td></tr>
            {(notes != null ? $"<tr><td style='padding:8px;border:1px solid #ddd'><b>Notes</b></td><td style='padding:8px;border:1px solid #ddd'>{notes}</td></tr>" : "")}
          </table>
        </body></html>
        """;
}
