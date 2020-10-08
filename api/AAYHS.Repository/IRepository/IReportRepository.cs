using System;
using System.Collections.Generic;
using System.Text;
using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;

namespace AAYHS.Repository.IRepository
{
    public interface IReportRepository
    {
        GetExhibitorRegistrationReport GetExhibitorRegistrationReport(RegistrationReportRequest registrationReportRequest);
    }
}
