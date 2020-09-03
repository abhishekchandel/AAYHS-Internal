using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.IService
{
    public interface IStallAssignmentService
    {
        MainResponse GetAllAssignedStalls();
        MainResponse DeleteAssignedStall(int StallAssignmentId);
    }
}
