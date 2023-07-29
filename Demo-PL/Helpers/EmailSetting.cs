using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo_PL.Helpers
{
	public static class EmailSetting
	{
		public static void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl= true;
			client.Credentials = new NetworkCredential("dinaahmed1012000@gmail.com", "iqdolcmexlergvzp");
			client.Send("dinaahmed1012000@gmail.com", email.To,email.Subject,email.Body);
		}
	}
}
