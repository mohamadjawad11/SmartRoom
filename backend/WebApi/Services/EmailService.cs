
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using System.Threading.Tasks;
using System.Linq;

namespace WebApi.Services
{
    public class EmailService
    {
        private readonly AppDbContext _context;

        public EmailService(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Send to all users in the database
        public async Task SendEmailToAllUsers(string subject, string bodyHtml)
        {
            var users = await _context.Users.ToListAsync();

            foreach (var user in users)
            {
                await SendAsync(user.Email, subject, bodyHtml);
            }
        }

        // ✅ Send to one email only (used for admin or affected user)
        public async Task SendAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Meeting App", "hamdanjawad789@gmail.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("hamdanjawad789@gmail.com", "uaps cyqo kpoc cytl"); // App password
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
