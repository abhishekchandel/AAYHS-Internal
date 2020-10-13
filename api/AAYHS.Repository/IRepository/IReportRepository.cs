using System;
using System.Collections.Generic;
using System.Text;
using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;

namespace AAYHS.Repository.IRepository
{
    public interface IReportRepository
    {
        GetExhibitorRegistrationReport GetExhibitorRegistrationReport(int exhibitorId);
        GetProgramReport GetProgramsReport(int classId);
        GetPaddockReport GetPaddockReport(int classId);
        GetAllClassesEntries GetAllClassesEntries();
    }
}
