using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.IService
{
    public interface IReportService
    {
        MainResponse GetExhibitorRegistrationReport(int exhibitorId);
        MainResponse GetProgramsReport(int classId);
        MainResponse GetPaddockReport(int classId);
        MainResponse GetAllClassesEntries();
    }
}
