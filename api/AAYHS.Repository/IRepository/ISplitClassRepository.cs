using AAYHS.Core.DTOs.Request;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
    public interface ISplitClassRepository:IGenericRepository<ClassSplits>
    {
        void DeleteSplitsByClassId(SplitRequest splitRequest);
    }
}
