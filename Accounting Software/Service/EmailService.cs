using Accounting_Software.Service_Interfaces;
using System.Net;
using System.Net.Mail;

namespace Accounting_Software.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            var host = _config["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
            var port = int.Parse(_config["EmailSettings:SmtpPort"] ?? "587");
            var senderEmail = _config["EmailSettings:SenderEmail"] ?? "";
            var senderPassword = _config["EmailSettings:SenderPassword"] ?? "";
            var senderName = _config["EmailSettings:SenderName"] ?? "Accounting Software";

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = "⚠️ Password Reset Request – Accounting Software",
                Body = $@"
<html>
<body style='font-family:Inter,sans-serif;background:#f0f2f5;padding:40px;'>
  <div style='max-width:520px;margin:auto;background:#fff;border-radius:14px;padding:36px;box-shadow:0 4px 15px rgba(0,0,0,0.08);'>
    <div style='text-align:center;margin-bottom:24px;'>
      <div style='width:64px;height:64px;border-radius:16px;background:linear-gradient(135deg,#f093fb,#f5576c);display:inline-flex;align-items:center;justify-content:center;'>
        <span style='font-size:1.8rem;'>🔑</span>
      </div>
    </div>
    <h2 style='color:#1a1a2e;margin-bottom:8px;text-align:center;'>Password Reset Request</h2>
    <p style='color:#6c757d;'>Someone (hopefully you) requested to reset the password for your <strong>Accounting Software</strong> account.</p>
    <div style='background:#fff8e1;border-radius:10px;padding:14px;margin:20px 0;'>
      <p style='color:#f57f17;margin:0;font-size:0.9rem;'>⚠️ <strong>If you did NOT request this</strong> — ignore this email. Your password will remain unchanged and the link expires in 1 hour.</p>
    </div>
    <p style='color:#1a1a2e;font-weight:600;'>If this was you, click the button below to approve the reset and set a new password:</p>
    <div style='text-align:center;margin:28px 0;'>
      <a href='{resetLink}' style='background:linear-gradient(135deg,#f093fb,#f5576c);color:#fff;padding:16px 36px;border-radius:10px;text-decoration:none;font-weight:700;font-size:1rem;display:inline-block;'>
        ✅ Approve &amp; Reset Password
      </a>
    </div>
    <p style='color:#aaa;font-size:0.82rem;text-align:center;'>This link expires in <strong>1 hour</strong>. Do not share it with anyone.</p>
  </div>
</body>
</html>",
                IsBodyHtml = true
            };
            mail.To.Add(toEmail);

            await client.SendMailAsync(mail);
        }
    }
}
