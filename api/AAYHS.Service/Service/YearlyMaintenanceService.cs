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
        private IAAYHSContactRepository _aAYHSContactRepository;
        private IRefundRepository _refundRepository;
        private IAAYHSContactAddressRepository _aAYHSContactAddressRepository;
        private IExhibitorPaymentDetailRepository _exhibitorPaymentDetailRepository;
        private IClassRepository _classRepository;
        private ISponsorExhibitorRepository _sponsorExhibitorRepository;
        private MainResponse _mainResponse;
        #endregion

        public YearlyMaintenanceService(IYearlyMaintenanceRepository yearlyMaintenanceRepository, IGlobalCodeRepository globalCodeRepository,
                                       IUserRepository userRepository,IYearlyMaintenanceFeeRepository yearlyMaintenanceFeeRepository,
                                       IEmailSenderRepository emailRepository,
                          IApplicationSettingRepository applicationRepository,IRoleRepository roleRepository ,
                          IUserRoleRepository userRoleRepository,IAAYHSContactRepository aAYHSContactRepository,
                          IRefundRepository refundRepository,IAAYHSContactAddressRepository aAYHSContactAddressRepository,
                          IExhibitorPaymentDetailRepository exhibitorPaymentDetailRepository,
                          IClassRepository classRepository,ISponsorExhibitorRepository sponsorExhibitorRepository,IMapper Mapper)
        {
            _yearlyMaintenanceRepository = yearlyMaintenanceRepository;
            _globalCodeRepository = globalCodeRepository;
            _userRepository = userRepository;
            _yearlyMaintenanceFeeRepository = yearlyMaintenanceFeeRepository;
            _emailRepository = emailRepository;
            _applicationRepository = applicationRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _aAYHSContactRepository = aAYHSContactRepository;
            _refundRepository = refundRepository;
            _aAYHSContactAddressRepository = aAYHSContactAddressRepository;
            _exhibitorPaymentDetailRepository = exhibitorPaymentDetailRepository;
            _classRepository = classRepository;
            _sponsorExhibitorRepository = sponsorExhibitorRepository;
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
                    Date = DateTime.Now,
                    Location = addYearly.Location,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now

                };

               int newId= _yearlyMaintenanceRepository.Add(newYearly).YearlyMaintainenceId;
                _mainResponse.Success = true;
                _mainResponse.NewId = newId;
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

        public MainResponse DeleteAdFee(DeleteAdFee deleteAd ,string actionBy)
        {
            var deleteAdFee = _yearlyMaintenanceFeeRepository.GetSingle(x => x.YearlyMaintainenceFeeId == deleteAd.YearlyMaintenanceFeeId);

            if (deleteAdFee!=null)
            {
                var checkFeeType = _sponsorExhibitorRepository.GetSingle(x => x.AdTypeId == deleteAdFee.FeeTypeId && x.IsDeleted == false);

                if (checkFeeType!=null)
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.FEE_ALREADY_IN_USE;
                    return _mainResponse;
                }
               
                deleteAdFee.IsDeleted = true;
                deleteAdFee.DeletedBy = actionBy;
                deleteAdFee.DeletedDate = DateTime.Now;
                _yearlyMaintenanceFeeRepository.Update(deleteAdFee);

                var getGlobalCode = _globalCodeRepository.GetSingle(x => x.GlobalCodeId == deleteAd.AdSizeId);
                if (getGlobalCode != null)
                {
                    getGlobalCode.IsDeleted = true;
                    getGlobalCode.DeletedBy = actionBy;
                    getGlobalCode.DeletedDate = DateTime.Now;

                    _globalCodeRepository.Update(getGlobalCode);
                }
               
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
            var checkClassCategory = _globalCodeRepository.GetSingle(x => x.CodeName.ToLower() == addClassCategoryRequest.CategoryName.ToLower() 
            && x.CategoryId==categoryId && x.IsActive == true & x.IsDeleted == false);

            if (checkClassCategory!=null)
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.RECORD_AlREADY_EXIST;
                return _mainResponse;
            }
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
                var checkClassCategory = _classRepository.GetSingle(x => x.ClassHeaderId == classDelete.GlobalCodeId && x.IsDeleted == false);

                if (checkClassCategory!=null)
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.CLASS_CATEGORY_ALREADY_IN_USE;
                    return _mainResponse;
                }

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

            if (generalFees.getGeneralFeesResponses!=null)
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
            int feeTypeId = 0;
            int catgeoryId = _yearlyMaintenanceRepository.GetCategoryId("FeeType");

            var checkFeeTypeExist = _globalCodeRepository.GetSingle(x => x.CodeName.ToLower() == (addGeneralFeeRequest.FeeType != null ? addGeneralFeeRequest.FeeType.ToLower():"") &&
            x.CategoryId == catgeoryId && x.IsActive == true && x.IsDeleted == false) ;
            if (addGeneralFeeRequest.YearlyMaintainenceFeeId==0)
            {
                if (checkFeeTypeExist != null)
                {
                    if (checkFeeTypeExist.CodeName.ToLower() != "additional program" && addGeneralFeeRequest.TimeFrame=="")
                    {
                        _mainResponse.Success = false;
                        _mainResponse.Message = Constants.TIME_FRAME_REQUIRED;
                        return _mainResponse;
                    }
                    var checkFee = _yearlyMaintenanceFeeRepository.GetSingle(x => x.FeeTypeId == checkFeeTypeExist.GlobalCodeId &&
                    x.YearlyMaintainenceId == addGeneralFeeRequest.YearlyMaintainenceId && x.IsActive == true && x.IsDeleted == false);

                    if (checkFee != null)
                    {
                        if (checkFee.PreEntryFee != 0 && addGeneralFeeRequest.TimeFrame=="Pre")
                        {
                            _mainResponse.Success = false;
                            _mainResponse.Message = Constants.FEE_ALREADY_EXIST;
                            return _mainResponse;
                        }
                        if (checkFee.PostEntryFee != 0 && addGeneralFeeRequest.TimeFrame == "Post")
                        {
                            _mainResponse.Success = false;
                            _mainResponse.Message = Constants.FEE_ALREADY_EXIST;
                            return _mainResponse;
                        }
                        if (checkFee.Amount != 0 && addGeneralFeeRequest.TimeFrame == "")
                        {
                            _mainResponse.Success = false;
                            _mainResponse.Message = Constants.FEE_ALREADY_EXIST;
                            return _mainResponse;
                        }
                        if (checkFee.PreEntryFee!=0 && checkFee.PostEntryFee!=0 && addGeneralFeeRequest.TimeFrame == "")
                        {
                            _mainResponse.Success = false;
                            _mainResponse.Message = Constants.FEE_ALREADY_EXIST;
                            return _mainResponse;
                        }
                       
                        if (addGeneralFeeRequest.TimeFrame == "Pre")
                        {
                            checkFee.PreEntryFee = addGeneralFeeRequest.Amount;
                            checkFee.ModifiedBy = actionBy;
                            checkFee.ModifiedDate = DateTime.Now;

                            _yearlyMaintenanceFeeRepository.Update(checkFee);
                            _mainResponse.Success = true;
                            _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;

                        }
                        if (addGeneralFeeRequest.TimeFrame == "Post")
                        {

                            checkFee.PostEntryFee = addGeneralFeeRequest.Amount;
                            checkFee.ModifiedBy = actionBy;
                            checkFee.ModifiedDate = DateTime.Now;

                            _yearlyMaintenanceFeeRepository.Update(checkFee);
                            _mainResponse.Success = true;
                            _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;

                        }
                        if (addGeneralFeeRequest.TimeFrame == "")
                        {

                            checkFee.Amount = addGeneralFeeRequest.Amount;
                            checkFee.ModifiedBy = actionBy;
                            checkFee.ModifiedDate = DateTime.Now;

                            _yearlyMaintenanceFeeRepository.Update(checkFee);
                            _mainResponse.Success = true;
                            _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;

                        }
                    }
                    else
                    {
                        if (addGeneralFeeRequest.TimeFrame == "Pre")
                        {

                            var addGeneralFee = new YearlyMaintainenceFee
                            {
                                YearlyMaintainenceId = addGeneralFeeRequest.YearlyMaintainenceId,
                                FeeTypeId = checkFeeTypeExist.GlobalCodeId,
                                PreEntryFee = addGeneralFeeRequest.Amount,
                                PostEntryFee = 0,
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
                        if (addGeneralFeeRequest.TimeFrame == "Post")
                        {

                            var addGeneralFee = new YearlyMaintainenceFee
                            {
                                YearlyMaintainenceId = addGeneralFeeRequest.YearlyMaintainenceId,
                                FeeTypeId = checkFeeTypeExist.GlobalCodeId,
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
                                FeeTypeId = checkFeeTypeExist.GlobalCodeId,
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
                    }
                }
                else
                {
                    if (addGeneralFeeRequest.FeeType != "" && addGeneralFeeRequest.FeeType != null)
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

                        feeTypeId = _globalCodeRepository.Add(feeType).GlobalCodeId;
                    }
                    if (addGeneralFeeRequest.TimeFrame == "Pre")
                    {

                        var addGeneralFee = new YearlyMaintainenceFee
                        {
                            YearlyMaintainenceId = addGeneralFeeRequest.YearlyMaintainenceId,
                            FeeTypeId = feeTypeId,
                            PreEntryFee = addGeneralFeeRequest.Amount,
                            PostEntryFee = 0,
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

                }
            }
            else
            {                
                var getYearlyMain = _yearlyMaintenanceFeeRepository.GetSingle(x => x.YearlyMaintainenceFeeId == addGeneralFeeRequest.YearlyMaintainenceFeeId);
                
                if (getYearlyMain!=null)
                {
                    if (addGeneralFeeRequest.TimeFrame == "Pre")
                    {
                        getYearlyMain.PreEntryFee = addGeneralFeeRequest.Amount;
                        getYearlyMain.ModifiedBy = actionBy;
                        getYearlyMain.ModifiedDate = DateTime.Now;

                        _yearlyMaintenanceFeeRepository.Update(getYearlyMain);
                        _mainResponse.Success = true;
                        _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;

                    }
                    if (addGeneralFeeRequest.TimeFrame == "Post")
                    {

                        getYearlyMain.PostEntryFee = addGeneralFeeRequest.Amount;
                        getYearlyMain.ModifiedBy = actionBy;
                        getYearlyMain.ModifiedDate = DateTime.Now;

                        _yearlyMaintenanceFeeRepository.Update(getYearlyMain);
                        _mainResponse.Success = true;
                        _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;

                    }
                    if (addGeneralFeeRequest.TimeFrame == "")
                    {

                        getYearlyMain.Amount = addGeneralFeeRequest.Amount;
                        getYearlyMain.ModifiedBy = actionBy;
                        getYearlyMain.ModifiedDate = DateTime.Now;

                        _yearlyMaintenanceFeeRepository.Update(getYearlyMain);
                        _mainResponse.Success = true;
                        _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;

                    }
                }
                else
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;

                }
            }
                    
            return _mainResponse;
        }

        public MainResponse RemoveGeneralFee(RemoveGeneralFee removeGeneralFee, string actionBy)
        {
            var getGeneralFee = _yearlyMaintenanceFeeRepository.GetSingle(x => x.YearlyMaintainenceFeeId == removeGeneralFee.YearlyMaintenanceFeeId);

            if (getGeneralFee!=null)
            {
                var checkFee = _exhibitorPaymentDetailRepository.GetSingle(x => x.FeeTypeId == getGeneralFee.FeeTypeId && x.IsDeleted == false);

                if (checkFee!=null)
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.FEE_ALREADY_IN_USE;
                    return _mainResponse;
                }
                if (removeGeneralFee.TimeFrame=="Pre")
                {
                    getGeneralFee.PreEntryFee = 0;
                    _yearlyMaintenanceFeeRepository.Update(getGeneralFee);
                }

                if (removeGeneralFee.TimeFrame == "Post")
                {
                    getGeneralFee.PostEntryFee = 0;
                    _yearlyMaintenanceFeeRepository.Update(getGeneralFee);
                }

                if (removeGeneralFee.TimeFrame == "")
                {
                    getGeneralFee.Amount = 0;
                    _yearlyMaintenanceFeeRepository.Update(getGeneralFee);
                }
                if (getGeneralFee.PreEntryFee==0 && getGeneralFee.PostEntryFee==0 && getGeneralFee.Amount==0)
                {
                    getGeneralFee.IsDeleted = true;
                    getGeneralFee.DeletedBy = actionBy;
                    getGeneralFee.DeletedDate = DateTime.Now;
                    _yearlyMaintenanceFeeRepository.Update(getGeneralFee);

                    var getGlobalCode = _globalCodeRepository.GetSingle(x => x.GlobalCodeId == removeGeneralFee.FeeTypeId);
                    getGlobalCode.IsDeleted = true;
                    getGlobalCode.DeletedBy = actionBy;
                    getGlobalCode.DeletedDate = DateTime.Now;
                    _globalCodeRepository.Update(getGlobalCode);
                }
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

        public MainResponse  GetRefund(int yearlyMaintenanceId)
        {
            var getRefund = _yearlyMaintenanceRepository.GetAllRefund(yearlyMaintenanceId);

            if (getRefund.getRefunds!=null )
            {
                _mainResponse.GetAllRefund = getRefund;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }

            return _mainResponse;
        }

        public MainResponse AddRefund(AddRefundRequest addRefundRequest, string actionBy)
        {
            var addRefund = new RefundDetail
            {
                YearlyMaintenanceId=addRefundRequest.YearlyMaintenanceId,
                DateAfter= addRefundRequest.DateAfter,
                DateBefore=addRefundRequest.DateBefore,
                FeeTypeId=addRefundRequest.FeeTypeId,
                RefundPercentage=addRefundRequest.Refund,
                IsActive=true,
                IsDeleted=false,
                CreatedBy=actionBy,
                CreatedDate=DateTime.Now
            };

            _refundRepository.Add(addRefund);
            _mainResponse.Success = true;
            _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
            return _mainResponse;
        }

        public MainResponse RemoveRefund(int refundId,string actionBy)
        {
            var getRefund = _refundRepository.GetSingle(x => x.RefundDetailId == refundId);

            if (getRefund!=null)
            {
                getRefund.IsDeleted = true;
                getRefund.DeletedBy = actionBy;
                getRefund.DeletedDate = DateTime.Now;

                _refundRepository.Update(getRefund);
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
            _mainResponse.GetContactInfo = getContactInfo;
            _mainResponse.Success = true;         
            return _mainResponse;
        }

        public MainResponse AddUpdateContactInfo(AddContactInfoRequest addContactInfoRequest, string actionBy)
        {
            int exhibitorSponsorConfirmation=0;
            int exhibitorSponsorRefundStatement=0 ;
            int exhibitorConfirmationEntries=0;
            if (addContactInfoRequest.YearlyMaintenanceId!=0)
            {
                var yearlyMaint = _yearlyMaintenanceRepository.GetSingle(x => x.YearlyMaintainenceId == 
                addContactInfoRequest.YearlyMaintenanceId);

                if (yearlyMaint!=null)
                {                   
                    yearlyMaint.Location = addContactInfoRequest.Location;
                    _yearlyMaintenanceRepository.Update(yearlyMaint);
                }
            }
            if (addContactInfoRequest.AAYHSContactId==0)
            {

                var address1 = new AAYHSContactAddresses
                {
                    Address = addContactInfoRequest.ExhibitorSponsorAddress,
                    City = addContactInfoRequest.ExhibitorSponsorCity,
                    StateId = addContactInfoRequest.ExhibitorSponsorState,
                    ZipCode = addContactInfoRequest.ExhibitorSponsorZip,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };

                exhibitorSponsorConfirmation = _aAYHSContactAddressRepository.Add(address1).AAYHSContactAddressId;
                
              
                    var address2 = new AAYHSContactAddresses
                    {
                        Address = addContactInfoRequest.ExhibitorRefundAddress,
                        City = addContactInfoRequest.ExhibitorRefundCity,
                        StateId= addContactInfoRequest.ExhibitorRefundState,
                        ZipCode = addContactInfoRequest.ExhibitorRefundZip,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = actionBy,
                        CreatedDate = DateTime.Now

                    };

                exhibitorSponsorRefundStatement = _aAYHSContactAddressRepository.Add(address2).AAYHSContactAddressId;


                var address3 = new AAYHSContactAddresses
                {
                    Address = addContactInfoRequest.ReturnAddress,
                    City = addContactInfoRequest.ReturnCity,
                    StateId = addContactInfoRequest.ReturnState,
                    ZipCode = addContactInfoRequest.ReturnZip,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now

                };

                exhibitorConfirmationEntries = _aAYHSContactAddressRepository.Add(address3).AAYHSContactAddressId;

                var contactInfo = new AAYHSContact
                {
                    YearlyMaintainenceId = addContactInfoRequest.YearlyMaintenanceId,
                    Email1 = addContactInfoRequest.Email1,
                    Email2 = addContactInfoRequest.Email2,
                    Phone1 = addContactInfoRequest.Phone1,
                    Phone2 = addContactInfoRequest.Phone2,
                    ExhibitorSponsorConfirmationAddressId = exhibitorSponsorConfirmation,
                    ExhibitorSponsorRefundStatementAddressId = exhibitorSponsorRefundStatement,
                    ExhibitorConfirmationEntriesAddressId = exhibitorConfirmationEntries,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };

                _aAYHSContactRepository.Add(contactInfo);
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
            }
            else
            {
                var contact = _aAYHSContactRepository.GetSingle(x => x.AAYHSContactId == addContactInfoRequest.AAYHSContactId);
               
                    var address1 = _aAYHSContactAddressRepository.GetSingle(x => x.AAYHSContactAddressId ==
                    contact.ExhibitorSponsorConfirmationAddressId);

                if (address1 != null)
                {
                    address1.Address = addContactInfoRequest.ExhibitorSponsorAddress;
                    address1.City = addContactInfoRequest.ExhibitorSponsorCity;
                    address1.StateId = addContactInfoRequest.ExhibitorSponsorState;
                    address1.ZipCode = addContactInfoRequest.ExhibitorSponsorZip;
                    address1.ModifiedBy = actionBy;
                    address1.ModifiedDate = DateTime.Now;

                    _aAYHSContactAddressRepository.Update(address1);
                }
                                       
             
                    var address2 = _aAYHSContactAddressRepository.GetSingle(x => x.AAYHSContactAddressId ==
                    contact.ExhibitorSponsorRefundStatementAddressId);

                    if (address2 != null)
                    {
                    address2.Address = addContactInfoRequest.ExhibitorRefundAddress;
                    address2.City= addContactInfoRequest.ExhibitorRefundCity;
                    address2.StateId = addContactInfoRequest.ExhibitorRefundState;
                    address2.ZipCode = addContactInfoRequest.ExhibitorRefundZip;
                    address2.ModifiedBy = actionBy;
                    address2.ModifiedDate = DateTime.Now;

                        _aAYHSContactAddressRepository.Update(address2);
                    }

               
                    var address = _aAYHSContactAddressRepository.GetSingle(x => x.AAYHSContactAddressId ==
                    contact.ExhibitorConfirmationEntriesAddressId);

                if (address != null)
                {
                    address.Address = addContactInfoRequest.ReturnAddress;
                    address.City = addContactInfoRequest.ReturnCity;
                    address.StateId = addContactInfoRequest.ReturnState;
                    address.ZipCode = addContactInfoRequest.ReturnZip;
                    address.ModifiedBy = actionBy;
                    address.ModifiedDate = DateTime.Now;

                    _aAYHSContactAddressRepository.Update(address);
                }
                var contactInfo = _aAYHSContactRepository.GetSingle(x => x.AAYHSContactId == addContactInfoRequest.AAYHSContactId &&
                x.IsActive==true && x.IsDeleted==false);

                if (contactInfo!=null)
                {
                    contactInfo.YearlyMaintainenceId = addContactInfoRequest.YearlyMaintenanceId;
                    contactInfo.Email1 = addContactInfoRequest.Email1;
                    contactInfo.Email2 = addContactInfoRequest.Email2;
                    contactInfo.Phone1 = addContactInfoRequest.Phone1;
                    contactInfo.Phone2 = addContactInfoRequest.Phone2;
                    contactInfo.ExhibitorSponsorConfirmationAddressId = contact.ExhibitorSponsorConfirmationAddressId;
                    contactInfo.ExhibitorSponsorRefundStatementAddressId = contact.ExhibitorSponsorRefundStatementAddressId;
                    contactInfo.ExhibitorConfirmationEntriesAddressId = contact.ExhibitorConfirmationEntriesAddressId;
                    contactInfo.ModifiedBy = actionBy;
                    contactInfo.ModifiedDate = DateTime.Now;

                    _aAYHSContactRepository.Update(contactInfo);

                    _mainResponse.Success = true;
                    _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
                }
                
            }
            return _mainResponse;
        }

        public MainResponse AddUpdateLocation(AddLocationRequest addLocationRequest, string actionBy)
        {
            int addressId = 0;
            var yearly = _yearlyMaintenanceRepository.GetSingle(x => x.YearlyMaintainenceId == addLocationRequest.YearlyMaintenanceId);
            if (yearly.LocationAddressId == null)
            {
                var address = new AAYHSContactAddresses
                {
                    Address = addLocationRequest.Address,
                    City = addLocationRequest.City,
                    StateId = addLocationRequest.StateId,
                    ZipCode = addLocationRequest.ZipCode,
                    Phone = addLocationRequest.Phone,
                    IsActive=true,
                    IsDeleted=false,
                    CreatedBy=actionBy,
                    CreatedDate=DateTime.Now

                };
                addressId = _aAYHSContactAddressRepository.Add(address).AAYHSContactAddressId;              
                if (yearly != null)
                {
                    yearly.Location = addLocationRequest.Name;
                    yearly.LocationAddressId = addressId;
                    yearly.ModifiedBy = actionBy;
                    yearly.ModifiedDate = DateTime.Now;

                    _yearlyMaintenanceRepository.Update(yearly);
                }
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
            }
            else
            {
                var addressUpdate = _aAYHSContactAddressRepository.GetSingle(x => x.AAYHSContactAddressId == yearly.LocationAddressId);

                if (addressUpdate!=null)
                {
                    addressUpdate.Address = addLocationRequest.Address;
                    addressUpdate.City = addLocationRequest.City;
                    addressUpdate.StateId = addLocationRequest.StateId;
                    addressUpdate.ZipCode = addLocationRequest.ZipCode;
                    addressUpdate.Phone = addLocationRequest.Phone;
                    addressUpdate.ModifiedBy = actionBy;
                    addressUpdate.ModifiedDate = DateTime.Now;

                    _aAYHSContactAddressRepository.Update(addressUpdate);
                }

                if (yearly != null)
                {
                    yearly.Location = addLocationRequest.Name;
                    yearly.LocationAddressId = yearly.LocationAddressId;
                    yearly.ModifiedBy = actionBy;
                    yearly.ModifiedDate = DateTime.Now;

                    _yearlyMaintenanceRepository.Update(yearly);
                }

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
            }

            return _mainResponse;
        }

        public MainResponse GetLocation(int yearlyMaintenanceId)
        {
            var getLocation = _yearlyMaintenanceRepository.GetLocation(yearlyMaintenanceId);

            if (getLocation!=null)
            {
                _mainResponse.GetLocation = getLocation;
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
