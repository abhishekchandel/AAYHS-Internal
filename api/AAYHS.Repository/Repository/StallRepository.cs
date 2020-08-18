using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.Repository
{
    public class StallRepository:GenericRepository<Stall>,IStallRepository
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region Private
        private MainResponse _MainResponse;       
        #endregion

        #region public
        public AAYHSDBContext _ObjContext;
        #endregion

        public StallRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _MainResponse = new MainResponse();
            _ObjContext = ObjContext;
            _Mapper = Mapper;
        }

        public GetAllStall GetAllStall()
        {
            GetAllStall getAllStall = new GetAllStall();
            var stall = (from stalls in _ObjContext.Stall
                         join stallAssign in _ObjContext.StallAssignment on stalls.StallId equals stallAssign.StallId into stallAssign1
                         from stallAssign2 in stallAssign1.DefaultIfEmpty()
                         where stalls.IsActive == true && stalls.IsDeleted == false
                         select new StallResponse
                         {
                             StallId = stalls.StallId,
                             StallNumber = stalls.StallNumber,
                             IsPortable = stalls.IsPortable,
                             ProtableStallTypeId = stalls.ProtableStallTypeId,
                             IsBooked = stallAssign2 != null ? true : false,
                             BookedById = stallAssign2 != null ? (stallAssign2.BookedByType == "Group" ? stallAssign2.GroupId : stallAssign2.ExhibitorId) : 0,
                             BookedByType= stallAssign2 != null ? stallAssign2.BookedByType : "",
                             BookedByName = stallAssign2 != null ? (stallAssign2.BookedByType == "Group" ?
                                           _ObjContext.Groups.Where(x => x.GroupId == stallAssign2.GroupId).Select(x => x.GroupName).FirstOrDefault() :
                                           _ObjContext.Exhibitors.Where(x => x.ExhibitorId == stallAssign2.ExhibitorId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault()) : "",
                         }).ToList();

            getAllStall.stallResponses = stall.ToList();
            getAllStall.TotalRecords = stall.Count();
            return getAllStall;
        }

    }
}
