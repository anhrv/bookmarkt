using MailKit.Net.Smtp;
using MimeKit;

namespace backend.Services
{
	public class EmailService
	{
		private readonly IConfiguration _config;

		public EmailService(IConfiguration config)
		{
			_config = config;
		}

		public void Send(string to, string subject, string body)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(_config["Email:KorisnickoIme"], _config["Email:From"]));
			message.To.Add(new MailboxAddress("", to));
			message.Subject = subject;

			var bodyBuilder = new BodyBuilder { HtmlBody = body };
			message.Body = bodyBuilder.ToMessageBody();

			using (var client = new SmtpClient())
			{
				client.Connect(_config["Email:Server"], Int32.Parse(_config["Email:Port"]), false);
				client.Authenticate(_config["Email:From"], _config["Email:Lozinka"]);
				client.Send(message);
				client.Disconnect(true);
			}
		}
	}
}
