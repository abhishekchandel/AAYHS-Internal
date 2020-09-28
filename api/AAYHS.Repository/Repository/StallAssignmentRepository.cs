using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection.Metadata;
using AAYHS.Core.Shared.Static;

namespace AAYHS.Repository.Repository
{
    public class StallAssignmentRepository: GenericRepository<StallAssignment>, IStallAssignmentRepository
    {
        #region readonly
        private readonly AAYHSDBContext _ObjContext;
        private IMapper _Mapper;
        #endregion

        #region private 
        private MainResponse _MainResponse;
        #endregion

        public StallAssignmentRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _MainResponse = new MainResponse();
            _ObjContext = ObjContext;
            _Mapper = Mapper;
        }
        public GetAllStall GetAllAssignedStalls()
        {
            GetAllStall getAllStall = new GetAllStall();
            var stall = (from stallAssign in _ObjContext.StallAssignment
                         where stallAssign.IsActive == true && stallAssign.IsDeleted == false
                         select new StallResponse
                         {
                             StallAssignmentId = stallAssign.StallAssignmentId,
                             StallId = stallAssign.StallId,
                             StallAssignmentTypeId = stallAssign.StallAssignmentTypeId,
                             GroupId = stallAssign.GroupId,
                             ExhibitorId = stallAssign.ExhibitorId,
                             BookedByType = stallAssign.BookedByType,
                             BookedByName= stallAssign.BookedByType=="Group"? 
                             (from grp in _ObjContext.Groups where grp.GroupId == stallAssign.GroupId 
                              select grp.GroupName).FirstOrDefault():
                             (from exb in _ObjContext.Exhibitors where exb.ExhibitorId == stallAssign.ExhibitorId 
                              select exb.FirstName + ' '+ exb.LastName).FirstOrDefault()

                         }).ToList();

            getAllStall.stallResponses = stall.ToList();
            getAllStall.TotalRecords = stall.Count();
            return getAllStall;
        }

        public MainResponse RemoveAllGroupAssignedStalls(int GroupId)
        {
            
         var list=   _ObjContext.StallAssignment.Where(x => x.GroupId == GroupId).ToList();
            if (list.Count > 0)
            {
                _ObjContext.StallAssignment.RemoveRange(list);
                _ObjContext.SaveChanges();
                _MainResponse.Success = true;
                _MainResponse.Message = Constants.RECORD_DELETE_SUCCESS;
            }
            return _MainResponse;
        }

        public MainResponse RemoveAllExhibitorAssignedStalls(int ExhibitorId)
        {

            var list = _ObjContext.StallAssignment.Where(x => x.ExhibitorId == ExhibitorId).ToList();
            if (list.Count > 0)
            {
                _ObjContext.StallAssignment.RemoveRange(list);
                _ObjContext.SaveChanges();
                _MainResponse.Success = true;
                _MainResponse.Message = Constants.RECORD_DELETE_SUCCESS;
            }
            return _MainResponse;
        }

    }
}
