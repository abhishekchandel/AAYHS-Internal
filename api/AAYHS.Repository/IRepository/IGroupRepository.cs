using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
   public interface IGroupRepository : IGenericRepository<Groups>
    {
        MainResponse GetGroupById(int GroupId);
        MainResponse GetAllGroups(BaseRecordFilterRequest request);
        GroupListResponse SearchGroup(SearchRequest searchRequest);
        GetAllGroupExhibitors GetGroupExhibitors(int GroupId);
        GetAllGroupFinacials GetAllGroupFinancials(int GroupId);
        GetGroupStatement GetGroupStatement(int GroupId);
    }
}
