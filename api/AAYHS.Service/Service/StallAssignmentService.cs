using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Static;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AAYHS.Service.Service
{
    public class StallAssignmentService: IStallAssignmentService
    {       
        #region private
        private MainResponse _mainResponse;
        private IMapper _mapper;
        private IStallAssignmentRepository _stallAssignmentRepository;
        #endregion

        public StallAssignmentService(IMapper Mapper,IStallAssignmentRepository stallAssignmentRepository)
        {
            _mapper = Mapper;
            _stallAssignmentRepository = stallAssignmentRepository;
            _mainResponse = new MainResponse();
        }

        public MainResponse GetAllAssignedStalls()
        {
            var getAllStall = _stallAssignmentRepository.GetAllAssignedStalls();
            if (getAllStall!=null && getAllStall.TotalRecords!=0)
            {
                _mainResponse.GetAllStall = getAllStall;
                _mainResponse.GetAllStall.TotalRecords = getAllStall.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = true;
            }
            return _mainResponse;
        }

        public MainResponse DeleteAssignedStall(int StallAssignmentId)
        {
            var stallAssign = _stallAssignmentRepository.GetSingle(x => x.StallAssignmentId == StallAssignmentId);
            if (stallAssign != null)
            {
              
                _stallAssignmentRepository.Delete(stallAssign);
                _mainResponse.Message = Constants.RECORD_DELETE_SUCCESS;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

    }
}
