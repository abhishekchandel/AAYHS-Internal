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
            mail.Subject = "Contact us";
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
            if (request.TemplateType == "Forget Password")
            {
                StringBuilder emailMessage = new StringBuilder();

                emailMessage.Append("<p>Hello,</p>");
                emailMessage.Append("<p style='margin-left:10%;margin-top:5%'>You have requested a password reset for the AAYHS website.</p>");
                emailMessage.Append(string.Format("<p style='margin-left:10%;'><a href='{0}?email={1}&token={2}'>Please click here to change your password</a></p>", request.Url, request.To, request.guid));
                emailMessage.Append("<p style='margin-left:10%;margin-bottom:5%'>If you did not request a password reset, please just ignore this email.");
                emailMessage.Append("<p>Thank you.</p>");

                Body = "<html>" +
                    "<body>" +
                    "<b>Sender Name : </b>" + request.Name + "<br/>" +
                    "<b>Sender Email : </b>" + request.SenderEmail + "<br/>" +
                    " <b>Message : </b>" + emailMessage + "<br/>" +
                    "<p>AAYHS  </br></p>" +
                    "</body>" +
                    "</html>";
            }
            return Body;
        }
    }
}
