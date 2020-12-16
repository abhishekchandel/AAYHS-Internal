using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
    public interface IYearlyStatementTextRepository:IGenericRepository<YearlyStatementText>
    {
        GetAllStatementText GetYearlyStatementText(int yearlyMaintenanceId);
    }
}
