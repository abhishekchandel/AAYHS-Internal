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
                             BookedByType = stallAssign.BookedByType

                         }).ToList();

            getAllStall.stallResponses = stall.ToList();
            getAllStall.TotalRecords = stall.Count();
            return getAllStall;
        }
    }
}
