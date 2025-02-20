using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ToDoList.EmailService;

public class Email
{
    // Mailtrap SMTP settings
    private readonly string _smtpServer = "smtp-relay.brevo.com";
    private readonly int _port = 587; // Or 25, 465, 2525 depending on what you want to use
    private readonly string _username = "csharpnetframwork@gmail.com";
    private readonly string _password = "KfpJ6dbQECk0sLS1_Test"; // Replace with your actual password

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient(_smtpServer, _port)
        {
            Credentials = new NetworkCredential(_username, _password),
            EnableSsl = true // Mailtrap uses SSL/TLS, so this should be true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("csharpnetframwork@gmail.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
            Console.WriteLine("Email sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
}
