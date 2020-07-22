using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Repository.IRepository
{
    public interface IGlobalCodeRepository: IGenericRepository<GlobalCodes>
    {
        Task<GlobalCodeMainResponse> GetCodes(string categoryName);
    }
}
