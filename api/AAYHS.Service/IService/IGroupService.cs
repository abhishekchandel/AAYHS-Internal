using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.IService
{
   public interface IGroupService
    {
        MainResponse AddUpdateGroup(GroupRequest request);
        MainResponse GetAllGroups(BaseRecordFilterRequest request);
        MainResponse GetGroupById(int SponsorId);
        MainResponse DeleteGroup(int SponsorId);
        MainResponse SearchGroup(SearchRequest searchRequest);
        MainResponse GetGroupExhibitors(int GroupId);
        MainResponse DeleteGroupExhibitor(int groupExhibitorId, string actionBy);
        MainResponse AddUpdateGroupFinancials(AddGroupFinancialRequest addGroupFinancialRequest, string actionBy);
        MainResponse DeleteGroupFinancials(int groupFinancialId, string actionBy);
        MainResponse GetAllGroupFinancials(int GroupId);
        MainResponse GetGroupStatement(int GroupId);
    }
}
