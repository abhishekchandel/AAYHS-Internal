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
            var getAllStall = _stallRepository.GetAllStall();
            if (getAllStall!=null && getAllStall.TotalRecords!=0)
            {
                _mainResponse.GetAllStall = getAllStall;
                _mainResponse.GetAllStall.TotalRecords = getAllStall.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse DeleteStallAssignment(int StallAssignmentId)
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
