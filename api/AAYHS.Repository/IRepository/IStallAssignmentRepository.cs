using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
    public interface IStallAssignmentRepository: IGenericRepository<StallAssignment>
    {
        GetAllStall GetAllAssignedStalls();
        MainResponse RemoveAllGroupAssignedStalls(int GroupId);
    }
}
