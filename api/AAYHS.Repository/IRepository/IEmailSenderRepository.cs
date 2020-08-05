using AAYHS.Core.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
    public interface IEmailSenderRepository
    {
        void SendEmail(EmailRequest request);
    }
}
