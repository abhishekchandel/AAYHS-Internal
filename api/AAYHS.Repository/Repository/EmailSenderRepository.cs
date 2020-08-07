using AAYHS.Core.DTOs.Request;
using AAYHS.Repository.IRepository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace AAYHS.Repository.Repository
{
    public class EmailSenderRepository: IEmailSenderRepository
    {
        #region IOC Containers
        protected readonly IConfiguration _configuration;
        #endregion

        public EmailSenderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(EmailRequest request)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(request.To);

            mail.From = new MailAddress(request.CompanyEmail);
            mail.Subject = "Reset Password";
            mail.Body = String.Format(Templates(request), request.To);
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = _configuration.GetSection(key: "EmailCredentails:Host").Value;
            smtp.Port = Convert.ToInt32(_configuration.GetSection(key: "EmailCredentails:Port").Value);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(request.CompanyEmail, request.CompanyPassword);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        public string Templates(EmailRequest request)
        {
            string Body = "";
            if (request.TemplateType == "Forgot Password")
            {
                Body = "<html>" +
                    "<body>" +
                    " <p> You have requested a password reset for the EveryObit website.</p> " + "" +
                    string.Format("<p><a href='{0}?email={1}&token={2}'> Please click here to reset your password</a></p>", request.Url, request.To, request.Token) + "" +
                    "<p> If you did not request a password reset, please just ignore this email." + "" +
                    "<p>Thank you for using Everyobit  </br></p>" +
                    "</body>" +
                    "</html>";
            }
            return Body;
        }
    }
}
