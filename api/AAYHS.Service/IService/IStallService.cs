using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.IService
{
    public interface IStallService
    {
        MainResponse GetAllStall();
        MainResponse DeleteStallAssignment(int StallAssignmentId);
    }
}
