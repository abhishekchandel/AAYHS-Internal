using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Static;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AAYHS.Service.Service
{
    public class StallService: IStallService
    {       
        #region private
        private MainResponse _mainResponse;
        private IMapper _mapper;
        private IStallRepository _stallRepository;
        private IStallAssignmentRepository _stallAssignmentRepository;
        private StallService _stallService;
        #endregion

        public StallService(IMapper Mapper,IStallRepository stallRepository,IStallAssignmentRepository stallAssignmentRepository)
        {
            _mapper = Mapper;
            _stallRepository = stallRepository;
            _stallAssignmentRepository = stallAssignmentRepository;
            _mainResponse = new MainResponse();
        }

        public MainResponse GetAllStall()
        {
            GetAllStall getAllStall = new GetAllStall();
            var allStall = _stallRepository.GetAll(x => x.IsActive == true && x.IsDeleted == false);
            var stallAssign = _stallAssignmentRepository.GetAll(x => x.IsActive == true && x.IsDeleted == false);
                                        
            if (allStall.Count!=0)
            {
                for (int i = 0; i < allStall.Count(); i++)
                {
                    getAllStall.stallResponses[i].StallId = allStall[i].StallId;
                    getAllStall.stallResponses[i].StallNumber = allStall[i].StallNumber;
                    getAllStall.stallResponses[i].IsPortable = allStall[i].IsPortable;
                    getAllStall.stallResponses[i].ProtableStallTypeId = allStall[i].ProtableStallTypeId;
                    getAllStall.stallResponses[i].IsBooked = stallAssign != null ? stallAssign.Where(x => x.StallId == allStall[i].StallId).Any() : false;
                    getAllStall.stallResponses[i].BookedById = stallAssign != null ? stallAssign.Where(x => x.StallId == allStall[i].StallId && x.BookedBy == "Group").Select(x => x.GroupId).FirstOrDefault() : stallAssign.Where(x => x.StallId == allStall[i].StallId && x.BookedBy == "Exhibitor").Select(x => x.ExhibitorId).FirstOrDefault();
                }

                _mainResponse.GetAllStall = getAllStall;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message= Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
    }
}
