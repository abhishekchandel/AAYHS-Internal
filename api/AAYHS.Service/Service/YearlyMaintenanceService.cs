using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;

namespace AAYHS.Service.Service
{
    public class YearlyMaintenanceService : IYearlyMaintenanceService
    {

        #region readonly       
        private IMapper _mapper;
        #endregion

        #region private
        private IYearlyMaintenanceRepository _yearlyMaintenanceRepository;
        private IGlobalCodeRepository _globalCodeRepository;
        private IUserRepository _userRepository;
        private MainResponse _mainResponse;
        #endregion

        public YearlyMaintenanceService(IYearlyMaintenanceRepository yearlyMaintenanceRepository, IGlobalCodeRepository globalCodeRepository,
                                       IUserRepository userRepository, IMapper Mapper)
        {
            _yearlyMaintenanceRepository = yearlyMaintenanceRepository;
            _globalCodeRepository = globalCodeRepository;
            _userRepository = userRepository;
            _mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse GetAllYearlyMaintenance(GetAllYearlyMaintenanceRequest getAllYearlyMaintenanceRequest)
        {
            var feeType = _globalCodeRepository.GetCodes("FeeType");
            int feeTypeId = feeType.globalCodeResponse.Where(x => x.CodeName == "Administration").Select(x => x.GlobalCodeId).FirstOrDefault();
            var allYearlyMaintenance = _yearlyMaintenanceRepository.GetAllYearlyMaintenance(getAllYearlyMaintenanceRequest, feeTypeId);

            if (allYearlyMaintenance.getYearlyMaintenances != null && allYearlyMaintenance.TotalRecords > 0)
            {
                _mainResponse.GetAllYearlyMaintenance = allYearlyMaintenance;
                _mainResponse.GetAllYearlyMaintenance.TotalRecords = allYearlyMaintenance.TotalRecords;
                _mainResponse.Success = true;

            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }

        public MainResponse GetYearlyMaintenanceById(int yearlyMaintenanceId)
        {
            var yearlyMaintenance = _yearlyMaintenanceRepository.GetYearlyMaintenanceById(yearlyMaintenanceId);
            if (yearlyMaintenance != null)
            {
                _mainResponse.GetYearlyMaintenanceById = yearlyMaintenance;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
            }
            return _mainResponse;
        }

        public MainResponse GetAllUsers()
        {
            var users = _userRepository.GetAll(x => x.IsApproved == false && x.IsDeleted == false);

            if (users.Count() > 0)
            {
                var _users = _mapper.Map<List<GetUser>>(users);
                GetAllUsers getAllUsers = new GetAllUsers();
                getAllUsers.getUsers = _users;
                _mainResponse.GetAllUsers = getAllUsers;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }

        public MainResponse ApprovedUser(UserApprovedRequest userApprovedRequest, string actionBy)
        {
            var user = _userRepository.GetSingle(x => x.UserId == userApprovedRequest.UserId);

            if (user != null)
            {
                user.IsApproved = userApprovedRequest.IsApproved;
                user.ModifiedBy = actionBy;
                user.ModifiedDate = DateTime.Now;

                _userRepository.Update(user);
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.RECORD_UPDATE_FAILED;
            }
            return _mainResponse;
        }

        public MainResponse DeleteUser(int userId, string actionBy)
        {
            var user = _userRepository.GetSingle(x => x.UserId == userId);

            if (user != null)
            {
                user.IsDeleted = true;
                user.DeletedDate = DateTime.Now;
                user.DeletedBy = actionBy;

                _userRepository.Update(user);
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_DELETE_SUCCESS;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.RECORD_DELETE_FAILED;
            }
            return _mainResponse;
        }

        public MainResponse AddUpdateYearly(AddYearlyRequest addYearly, string actionBy)
        {
            if (addYearly.YearlyMaintainenceId == 0)
            {
                var yearExist = _yearlyMaintenanceRepository.GetSingle(x => x.Year== addYearly.Year && x.IsActive == true && x.IsDeleted == false);
                if (yearExist != null)
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.RECORD_AlREADY_EXIST;
                    return _mainResponse;
                }
                var newYearly = new YearlyMaintainence
                {
                    Year = addYearly.Year,
                    ShowStartDate = addYearly.ShowStartDate,
                    ShowEndDate = addYearly.ShowEndDate,
                    PreEntryCutOffDate = addYearly.PreCutOffDate,
                    SponcerCutOffDate = addYearly.SponcerCutOffDate,
                    Date = addYearly.Date,
                    Location = addYearly.Location,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now

                };

                _yearlyMaintenanceRepository.Add(newYearly);
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;

            }
            else
            {
                var year = _yearlyMaintenanceRepository.GetSingle(x =>x.YearlyMaintainenceId==addYearly.YearlyMaintainenceId && x.Year == addYearly.Year && x.IsActive == true && x.IsDeleted == false);

                if (year==null)
                {
                    var yearExist = _yearlyMaintenanceRepository.GetSingle(x => x.Year == addYearly.Year && x.IsActive == true && x.IsDeleted == false);
                    if (yearExist != null)
                    {
                        _mainResponse.Success = false;
                        _mainResponse.Message = Constants.RECORD_AlREADY_EXIST;
                        return _mainResponse;
                    }
                }

                var updateYear = _yearlyMaintenanceRepository.GetSingle(x => x.YearlyMaintainenceId == addYearly.YearlyMaintainenceId);

                if (updateYear!=null)
                {
                    updateYear.Year = addYearly.Year;
                    updateYear.ShowStartDate = addYearly.ShowStartDate;
                    updateYear.ShowEndDate = addYearly.ShowEndDate;
                    updateYear.PreEntryCutOffDate = addYearly.PreCutOffDate;
                    updateYear.SponcerCutOffDate = addYearly.SponcerCutOffDate;
                    updateYear.Date = addYearly.Date;
                    updateYear.Location = addYearly.Location;
                    updateYear.ModifiedBy = actionBy;
                    updateYear.ModifiedDate = DateTime.Now;

                    _yearlyMaintenanceRepository.Update(updateYear);
                    _mainResponse.Success = true;
                    _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
                }
                else
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.NO_RECORD_Exist_WITH_ID;
                }
            }
            return _mainResponse;
        }

        public MainResponse DeleteYearly(int yearlyMaintainenceId,string actionBy)
        {
            var deleteYearly = _yearlyMaintenanceRepository.GetSingle(x => x.YearlyMaintainenceId == yearlyMaintainenceId);

            if (deleteYearly!=null)
            {
                deleteYearly.IsDeleted = true;
                deleteYearly.DeletedBy = actionBy;
                deleteYearly.DeletedDate = DateTime.Now;

                _yearlyMaintenanceRepository.Update(deleteYearly);
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_DELETE_SUCCESS;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
            }
            return _mainResponse;
        }

        public MainResponse AddADFee(AddAdFee addAdFee,string actionBy)
        {
            var feeType = _globalCodeRepository.GetCodes("AdTypes");

            var checkAdType = feeType.globalCodeResponse.Where(x => x.CodeName.ToLower() == addAdFee.AdSize.ToLower()).FirstOrDefault();

            if (checkAdType!=null)
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.RECORD_AlREADY_EXIST;
                return _mainResponse;
            }

            return null;
        }
    }
}
