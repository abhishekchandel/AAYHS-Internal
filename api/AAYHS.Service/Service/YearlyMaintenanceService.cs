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
        private IYearlyMaintenanceFeeRepository _yearlyMaintenanceFeeRepository;
        private IEmailSenderRepository _emailRepository;
        private IApplicationSettingRepository _applicationRepository;
        private IRoleRepository _roleRepository;
        private IUserRoleRepository _userRoleRepository;
        private MainResponse _mainResponse;
        #endregion

        public YearlyMaintenanceService(IYearlyMaintenanceRepository yearlyMaintenanceRepository, IGlobalCodeRepository globalCodeRepository,
                                       IUserRepository userRepository,IYearlyMaintenanceFeeRepository yearlyMaintenanceFeeRepository,
                                       IEmailSenderRepository emailRepository,
                          IApplicationSettingRepository applicationRepository,IRoleRepository roleRepository ,
                          IUserRoleRepository userRoleRepository,IMapper Mapper)
        {
            _yearlyMaintenanceRepository = yearlyMaintenanceRepository;
            _globalCodeRepository = globalCodeRepository;
            _userRepository = userRepository;
            _yearlyMaintenanceFeeRepository = yearlyMaintenanceFeeRepository;
            _emailRepository = emailRepository;
            _applicationRepository = applicationRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
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

                if (userApprovedRequest.IsApproved==true)
                {
                    string guid = Guid.NewGuid().ToString();
                  
                    var settings = _applicationRepository.GetAll().FirstOrDefault();

                    EmailRequest email = new EmailRequest();
                    email.To = user.Email;
                    email.SenderEmail = settings.CompanyEmail;
                    email.CompanyEmail = settings.CompanyEmail;
                    email.CompanyPassword = settings.CompanyPassword;
                    email.Url = settings.ResetPasswordUrl;
                    email.Token = guid;
                    email.TemplateType = "User Approved";

                    _emailRepository.SendEmail(email);

                    var role = new UserRoles
                    {
                        RoleId = userApprovedRequest.RoleId,
                        UserId = userApprovedRequest.UserId,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = actionBy,
                        CreatedDate = DateTime.Now
                    };

                    _userRoleRepository.Add(role);
                }

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
                var yearExist = _yearlyMaintenanceRepository.GetSingle(x => x.Years== addYearly.Year && x.IsActive == true && x.IsDeleted == false);
                if (yearExist != null)
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.RECORD_AlREADY_EXIST;
                    return _mainResponse;
                }
                var newYearly = new YearlyMaintainence
                {
                    Years = addYearly.Year,
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
                var year = _yearlyMaintenanceRepository.GetSingle(x =>x.YearlyMaintainenceId==addYearly.YearlyMaintainenceId && x.Years == addYearly.Year && x.IsActive == true && x.IsDeleted == false);

                if (year==null)
                {
                    var yearExist = _yearlyMaintenanceRepository.GetSingle(x => x.Years == addYearly.Year && x.IsActive == true && x.IsDeleted == false);
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
                    updateYear.Years = addYearly.Year;
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
           
            if (checkAdType != null)
            {
                var checkAdFee = _yearlyMaintenanceFeeRepository.GetSingle(x => x.YearlyMaintainenceId == addAdFee.YearlyMaintainenceId &&
                x.FeeTypeId == checkAdType.GlobalCodeId && x.IsActive == true && x.IsDeleted == false);

                if (checkAdFee == null)
                {
                    var addFee = new YearlyMaintainenceFee
                    {
                        YearlyMaintainenceId= addAdFee.YearlyMaintainenceId,
                        FeeTypeId=checkAdType.GlobalCodeId,
                        PreEntryFee=0,
                        PostEntryFee=0,
                        Amount= addAdFee.Amount,
                        RefundPercentage=40,
                        IsActive=true,
                        IsDeleted=false,
                        CreatedBy=actionBy,
                        CreatedDate=DateTime.Now

                    };

                    _yearlyMaintenanceFeeRepository.Add(addFee);
                    _mainResponse.Success = true;
                    _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
                }
                else
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.RECORD_AlREADY_EXIST;
                }
            }
            else
            {
                int categoryId = _yearlyMaintenanceRepository.GetCategoryId("AdTypes");
                var addGlobalCode = new GlobalCodes
                {
                    CategoryId = categoryId,
                    CodeName= addAdFee.AdSize,
                    Description= addAdFee.AdSize,
                    IsActive=true,
                    IsDeleted=false,
                    CreatedBy=actionBy,
                    CreatedDate=DateTime.Now
                };

               var codeId= _globalCodeRepository.Add(addGlobalCode);

                var addFee = new YearlyMaintainenceFee
                {
                    YearlyMaintainenceId = addAdFee.YearlyMaintainenceId,
                    FeeTypeId = codeId.GlobalCodeId,
                    PreEntryFee = 0,
                    PostEntryFee = 0,
                    Amount = addAdFee.Amount,
                    RefundPercentage = 40,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };
                _yearlyMaintenanceFeeRepository.Add(addFee);
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
            }

            return _mainResponse;
        }

        public MainResponse GetAllAdFees(int yearlyMaintenanceId)
        {
            var getallFees = _yearlyMaintenanceRepository.GetAllAdFees(yearlyMaintenanceId);

            if (getallFees.getAdFees!=null)
            {
                _mainResponse.GetAllAdFees = getallFees;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
            }
            return _mainResponse;
        }

        public MainResponse DeleteAdFee(int yearlyMaintenanceFeeId, string actionBy)
        {
            var deleteAdFee = _yearlyMaintenanceFeeRepository.GetSingle(x => x.YearlyMaintainenceFeeId == yearlyMaintenanceFeeId);

            if (deleteAdFee!=null)
            {
                deleteAdFee.IsDeleted = true;
                deleteAdFee.DeletedBy = actionBy;
                deleteAdFee.DeletedDate = DateTime.Now;

                _yearlyMaintenanceFeeRepository.Update(deleteAdFee);
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

        public MainResponse GetAllUsersApproved()
        {
            var users = _userRepository.GetAll(x => x.IsApproved == true && x.IsActive==true && x.IsDeleted == false);

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

        public MainResponse RemoveApprovedUser(int userId, string actionBy)
        {
            var deleteUser = _userRepository.GetSingle(x => x.UserId == userId);

            if (deleteUser!=null)
            {
                deleteUser.IsActive = false;
                deleteUser.IsDeleted = true;
                deleteUser.DeletedBy = actionBy;
                deleteUser.DeletedDate = DateTime.Now;

                _userRepository.Update(deleteUser);
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

        public MainResponse GetAllRoles()
        {
            var allRoles = _roleRepository.GetAll(x=>x.IsActive==true && x.IsDeleted==false);

            var _allRoles = _mapper.Map<List<GetRoles>>(allRoles);
            GetAllRoles getAllRoles = new GetAllRoles();
            getAllRoles.getRoles = _allRoles;
            _mainResponse.GetAllRoles = getAllRoles;
            _mainResponse.Success = true;

            return _mainResponse;
        }

        public MainResponse GetAllClassCategory()
        {
            var classCategory =_yearlyMaintenanceRepository.GetAllClassCategory();

            if (classCategory.getClassCategories!=null)
            {                
                _mainResponse.GetAllClassCategory = classCategory;
                _mainResponse.Success = true;

            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }

        public MainResponse AddClassCategory(AddClassCategoryRequest addClassCategoryRequest,string actionBy)
        {
            int categoryId = _yearlyMaintenanceRepository.GetCategoryId("ClassHeaderType");

            var classCategory = new GlobalCodes
            {
                CategoryId = categoryId,
                CodeName=addClassCategoryRequest.CategoryName,
                Description=addClassCategoryRequest.CategoryName,
                IsActive=true,
                IsDeleted=false,
                CreatedBy= actionBy,
                CreatedDate=DateTime.Now
            };

            _globalCodeRepository.Add(classCategory);
            _mainResponse.Success = true;
            _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
            return _mainResponse;
        }

        public MainResponse RemoveClassCategory(int globalCodeId, string actionBy)
        {
            var classDelete = _globalCodeRepository.GetSingle(x => x.GlobalCodeId == globalCodeId);

            if (classDelete!=null)
            {
                classDelete.IsDeleted = true;
                classDelete.DeletedBy = actionBy;
                classDelete.DeletedDate = DateTime.Now;

                _globalCodeRepository.Update(classDelete);

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

        public MainResponse GetAllGeneralFees(int yearlyMaintenanceId)
        {
            var generalFees = _yearlyMaintenanceRepository.GetAllGeneralFees(yearlyMaintenanceId);

            if (generalFees.getGeneralFees!=null)
            {
                _mainResponse.GetAllGeneralFees = generalFees;
                _mainResponse.Success = true;

            }
            else
            {                
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }

        public MainResponse AddGeneralFees(AddGeneralFeeRequest addGeneralFeeRequest, string actionBy)
        {
            int feeTypeId=0;
            int catgeoryId = _yearlyMaintenanceRepository.GetCategoryId("FeeType");

            if (addGeneralFeeRequest.FeeType!="" && addGeneralFeeRequest.FeeType != null)
            {
                var feeType = new GlobalCodes
                {
                    CategoryId = catgeoryId,
                    CodeName = addGeneralFeeRequest.FeeType,
                    Description = addGeneralFeeRequest.FeeType,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };

               feeTypeId=_globalCodeRepository.Add(feeType).GlobalCodeId;
            }
            if (addGeneralFeeRequest.TimeFrame=="Pre")
            {

                var addGeneralFee = new YearlyMaintainenceFee
                {
                    YearlyMaintainenceId = addGeneralFeeRequest.YearlyMaintainenceId,
                    FeeTypeId = feeTypeId,
                    PreEntryFee= addGeneralFeeRequest.Amount,
                    PostEntryFee=0,
                    Amount=0,
                    RefundPercentage=40,
                    IsActive=true,
                    IsDeleted=false,
                    CreatedBy=actionBy,
                    CreatedDate=DateTime.Now
                };

                _yearlyMaintenanceFeeRepository.Add(addGeneralFee);
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
               
            }
            if (addGeneralFeeRequest.TimeFrame == "Post")
            {
              
                var addGeneralFee = new YearlyMaintainenceFee
                {
                    YearlyMaintainenceId = addGeneralFeeRequest.YearlyMaintainenceId,
                    FeeTypeId = feeTypeId,
                    PreEntryFee = 0,
                    PostEntryFee = addGeneralFeeRequest.Amount,
                    Amount = 0,
                    RefundPercentage = 40,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now

                };

                _yearlyMaintenanceFeeRepository.Add(addGeneralFee);
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
                
            }
            if (addGeneralFeeRequest.TimeFrame == "")
            {
                
                var addGeneralFee = new YearlyMaintainenceFee
                {
                    YearlyMaintainenceId = addGeneralFeeRequest.YearlyMaintainenceId,
                    FeeTypeId = feeTypeId,
                    PreEntryFee = 0,
                    PostEntryFee = 0,
                    Amount = addGeneralFeeRequest.Amount,
                    RefundPercentage = 40,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };

                _yearlyMaintenanceFeeRepository.Add(addGeneralFee);
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
                
            }

            return _mainResponse;
        }

        public MainResponse RemoveGeneralFee(int yearlyMaintenanceFeeId, string actionBy)
        {
            var getGeneralFee = _yearlyMaintenanceFeeRepository.GetSingle(x => x.YearlyMaintainenceFeeId == yearlyMaintenanceFeeId);

            if (getGeneralFee!=null)
            {
                getGeneralFee.IsDeleted = true;
                getGeneralFee.DeletedBy = actionBy;
                getGeneralFee.DeletedDate = DateTime.Now;

                _yearlyMaintenanceFeeRepository.Update(getGeneralFee);
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

        public MainResponse GetContactInfo(int yearlyMaintenanceId)
        {
            var getContactInfo = _yearlyMaintenanceRepository.GetContactInfo(yearlyMaintenanceId);

            if (getContactInfo != null)
            {
                _mainResponse.GetContactInfo = getContactInfo;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }

            return _mainResponse;
        }
    }
}
