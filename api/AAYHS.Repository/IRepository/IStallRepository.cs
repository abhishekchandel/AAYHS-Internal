using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
    public interface IStallRepository:IGenericRepository<Stall>
    {
        GetAllStall GetAllStall();
    }
}
