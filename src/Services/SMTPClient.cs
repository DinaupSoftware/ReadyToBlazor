using System.Net;
using System.Net.Mail;

public class SMTPClient
{
	private SmtpSettings configSMTP;

	public SMTPClient(SmtpSettings configSMTP)
	{
		this.configSMTP = configSMTP;
	}

















	public async Task SendEmailAsync(string to, string subject, string body)
	{
		var smtpClient = new SmtpClient(configSMTP.Host)
		{
			Port = configSMTP.Port,
			Credentials = new NetworkCredential(configSMTP.UserName, configSMTP.Password),
			EnableSsl = configSMTP.EnableSsl,
		};

		var mailMessage = new MailMessage
		{
			From = new MailAddress(configSMTP.SenderEmail),
			Subject = subject,
			Body = body,
			IsBodyHtml = true,
		};
		mailMessage.To.Add(to);

		await smtpClient.SendMailAsync(mailMessage);
	}
}
