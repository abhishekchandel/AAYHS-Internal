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
using AAYHS.Core.DTOs.Response.Common;

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
        private IScanRepository _scan;
        private IYearlyStatementTextRepository _yearlyStatementTextRepository;
        private ISponsorIncentiveRepository _sponsorIncentiveRepository;
        private MainResponse _mainResponse;
        #endregion

        public YearlyMaintenanceService(IYearlyMaintenanceRepository yearlyMaintenanceRepository, IGlobalCodeRepository globalCodeRepository,
                                       IUserRepository userRepository,IYearlyMaintenanceFeeRepository yearlyMaintenanceFeeRepository,
                                       IEmailSenderRepository emailRepository,
                          IApplicationSettingRepository applicationRepository,IRoleRepository roleRepository ,
                          IUserRoleRepository userRoleRepository,IAAYHSContactRepository aAYHSContactRepository,
                          IRefundRepository refundRepository,IAAYHSContactAddressRepository aAYHSContactAddressRepository,
                          IExhibitorPaymentDetailRepository exhibitorPaymentDetailRepository,
                          IClassRepository classRepository,ISponsorExhibitorRepository sponsorExhibitorRepository,
                          IScanRepository scan,IYearlyStatementTextRepository yearlyStatementTextRepository,
                          ISponsorIncentiveRepository sponsorIncentiveRepository,IMapper Mapper)
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
            _scan = scan;
            _yearlyStatementTextRepository = yearlyStatementTextRepository;
            _sponsorIncentiveRepository = sponsorIncentiveRepository;
            _mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse GetAllYearlyMaintenance(GetAllYearlyMaintenanceRequest getAllYearlyMaintenanceRequest)
        {
            var feeType = _globalCodeRepository.GetCodes("FeeType");         
            var allYearlyMaintenance = _yearlyMaintenanceRepository.GetAllYearlyMaintenance(getAllYearlyMaintenanceRequest,feeType);

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
                                    
                    var settings = _applicationRepository.GetAll().FirstOrDefault();

                    EmailRequest email = new EmailRequest();
                    email.To = user.Email;
                    email.SenderEmail = settings.CompanyEmail;
                    email.CompanyEmail = settings.CompanyEmail;
                    email.CompanyPassword = settings.CompanyPassword;                  
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
                    ShowStartDate = Convert.ToDateTime(addYearly.ShowStartDate),
                    ShowEndDate = Convert.ToDateTime(addYearly.ShowEndDate),
                    PreEntryCutOffDate = Convert.ToDateTime(addYearly.PreCutOffDate),
                    SponcerCutOffDate = Convert.ToDateTime(addYearly.SponcerCutOffDate),
                    Date = DateTime.Now,
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
                    updateYear.ShowStartDate = Convert.ToDateTime(addYearly.ShowStartDate);
                    updateYear.ShowEndDate = Convert.ToDateTime(addYearly.ShowEndDate);
                    updateYear.PreEntryCutOffDate = Convert.ToDateTime(addYearly.PreCutOffDate);
                    updateYear.SponcerCutOffDate = Convert.ToDateTime(addYearly.SponcerCutOffDate);
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
                _yearlyMaintenanceRepository.DeleteYearlyFee(yearlyMaintainenceId);

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
            string adSize = "";
            if (addAdFee.AdSize!=string.Empty && addAdFee.AdSize!=null)
            {
                adSize = addAdFee.AdSize.TrimStart();
                adSize = adSize.TrimEnd();
            }
           
            var checkAdFee = _yearlyMaintenanceFeeRepository.GetSingle(x => x.YearlyMaintainenceId==addAdFee.YearlyMaintainenceId && 
                             x.FeeName.ToLower() == adSize.ToLower() && x.FeeType== "AdFee" &&  x.IsDeleted==false);

            if (checkAdFee!=null)
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.RECORD_AlREADY_EXIST;
                return _mainResponse;
            }
            var addFee = new YearlyMaintainenceFee
            {
                YearlyMaintainenceId = addAdFee.YearlyMaintainenceId,
                FeeType = "AdFee", 
                FeeName= adSize,
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
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
            }
            return _mainResponse;
        }

        public MainResponse DeleteAdFee(DeleteAdFee deleteAd ,string actionBy)
        {
            var deleteAdFee = _yearlyMaintenanceFeeRepository.GetSingle(x => x.YearlyMaintainenceFeeId == deleteAd.YearlyMaintenanceFeeId);

            if (deleteAdFee!=null)
            {
                var checkFeeType = _sponsorExhibitorRepository.GetSingle(x => x.AdTypeId == deleteAdFee.YearlyMaintainenceFeeId && x.IsDeleted == false);

                if (checkFeeType != null)
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.FEE_ALREADY_IN_USE;
                    return _mainResponse;
                }
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
            var users = _userRepository.GetAll(x => x.IsApproved == true && x.IsDeleted == false);

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
            string category = "";
            if (addClassCategoryRequest.CategoryName != string.Empty && addClassCategoryRequest.CategoryName != null)
            {
                category = addClassCategoryRequest.CategoryName.TrimStart();
                category = category.TrimEnd();
            }
            int categoryId = _yearlyMaintenanceRepository.GetCategoryId("ClassHeaderType");
            var checkClassCategory = _globalCodeRepository.GetSingle(x => x.CodeName.ToLower() == category.ToLower() 
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
                CodeName= category,
                Description= category,
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
                    
            if (addGeneralFeeRequest.YearlyMaintainenceFeeId==0)
            {
                string feeName = "";
                if (addGeneralFeeRequest.FeeType != string.Empty && addGeneralFeeRequest.FeeType != null)
                {
                    feeName = addGeneralFeeRequest.FeeType.TrimStart();
                    feeName = feeName.TrimEnd();
                }
                else
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.GENERAL_FEE_CANT_EMPTY;
                    return _mainResponse;
                }
                if (feeName.ToLower() != "additional programs" && addGeneralFeeRequest.TimeFrame == "")
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.TIME_FRAME_REQUIRED;
                    return _mainResponse;
                }
                var checkFeeNamePre = _yearlyMaintenanceFeeRepository.GetSingle(x => x.FeeType == "GeneralFee" && x.YearlyMaintainenceId == addGeneralFeeRequest.YearlyMaintainenceId
                                  && x.FeeName.ToLower() == feeName.ToLower() && x.TimeFrame == addGeneralFeeRequest.TimeFrame && x.IsDeleted == false);
                if (checkFeeNamePre != null)
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.RECORD_AlREADY_EXIST;
                    return _mainResponse;
                }               
                var checkProgramFee = _yearlyMaintenanceFeeRepository.GetSingle(x => x.FeeType == "GeneralFee" &&
                                      x.YearlyMaintainenceId == addGeneralFeeRequest.YearlyMaintainenceId && x.FeeName.ToLower() == "additional programs" &&
                                      x.IsDeleted == false);
                if (checkProgramFee != null && feeName.ToLower() == "additional programs")
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.RECORD_AlREADY_EXIST;
                    return _mainResponse;
                }
                var generalFee = new YearlyMaintainenceFee
                {
                    YearlyMaintainenceId = addGeneralFeeRequest.YearlyMaintainenceId,
                    FeeType = "GeneralFee",
                    TimeFrame = addGeneralFeeRequest.TimeFrame,
                    FeeName = feeName,
                    Amount = addGeneralFeeRequest.Amount,
                    RefundPercentage=40,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };

                _yearlyMaintenanceFeeRepository.Add(generalFee);
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
            }
            else
            {
                var generalFee = _yearlyMaintenanceFeeRepository.GetSingle(x => x.YearlyMaintainenceFeeId == addGeneralFeeRequest.YearlyMaintainenceFeeId);

                if (generalFee!=null)
                {
                    generalFee.Amount = addGeneralFeeRequest.Amount;
                    _yearlyMaintenanceFeeRepository.Update(generalFee);
                    _mainResponse.Success = true;
                    _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
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
                var checkFee = _exhibitorPaymentDetailRepository.GetSingle(x => x.FeeTypeId == getGeneralFee.YearlyMaintainenceFeeId && x.IsDeleted == false);

                if (checkFee != null)
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.FEE_ALREADY_IN_USE;
                    return _mainResponse;
                }
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
            
            if (addContactInfoRequest.AAYHSContactId==0)
            {

                var address1 = new AAYHSContactAddresses
                {
                    Address = addContactInfoRequest.ExhibitorSponsorAddress,
                    City = addContactInfoRequest.ExhibitorSponsorCity,
                    StateId = addContactInfoRequest.ExhibitorSponsorState,
                    ZipCode = addContactInfoRequest.ExhibitorSponsorZip,
                    Phone=addContactInfoRequest.ExhibitorSponsorPhone,
                    Email=addContactInfoRequest.ExhibitorSponsorEmail,
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
                    StateId = addContactInfoRequest.ExhibitorRefundState,
                    ZipCode = addContactInfoRequest.ExhibitorRefundZip,
                    Phone=addContactInfoRequest.ExhibitorRefundPhone,
                    Email=addContactInfoRequest.ExhibitorRefundEmail,
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
                    Phone=addContactInfoRequest.ReturnPhone,
                    Email=addContactInfoRequest.ReturnEmail,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now

                };

                exhibitorConfirmationEntries = _aAYHSContactAddressRepository.Add(address3).AAYHSContactAddressId;

                var contactInfo = new AAYHSContact
                {
                    YearlyMaintainenceId = addContactInfoRequest.YearlyMaintenanceId,
                    Location=addContactInfoRequest.Location,
                    Address=addContactInfoRequest.Address,
                    City=addContactInfoRequest.City,
                    State=addContactInfoRequest.State,
                    ZipCode=addContactInfoRequest.Zipcode,
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
                    address1.Phone = addContactInfoRequest.ExhibitorSponsorPhone;
                    address1.Email = addContactInfoRequest.ExhibitorSponsorEmail;
                    address1.ModifiedBy = actionBy;
                    address1.ModifiedDate = DateTime.Now;

                    _aAYHSContactAddressRepository.Update(address1);
                }


                var address2 = _aAYHSContactAddressRepository.GetSingle(x => x.AAYHSContactAddressId ==
                contact.ExhibitorSponsorRefundStatementAddressId);

                if (address2 != null)
                {
                    address2.Address = addContactInfoRequest.ExhibitorRefundAddress;
                    address2.City = addContactInfoRequest.ExhibitorRefundCity;
                    address2.StateId = addContactInfoRequest.ExhibitorRefundState;
                    address2.ZipCode = addContactInfoRequest.ExhibitorRefundZip;
                    address2.Phone = addContactInfoRequest.ExhibitorRefundPhone;
                    address2.Email = addContactInfoRequest.ExhibitorRefundEmail;
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
                    address.Phone = addContactInfoRequest.ReturnPhone;
                    address.Email = addContactInfoRequest.ReturnEmail;
                    address.ModifiedBy = actionBy;
                    address.ModifiedDate = DateTime.Now;

                    _aAYHSContactAddressRepository.Update(address);
                }
                var contactInfo = _aAYHSContactRepository.GetSingle(x => x.AAYHSContactId == addContactInfoRequest.AAYHSContactId &&
                x.IsActive==true && x.IsDeleted==false);

                if (contactInfo!=null)
                {
                    contactInfo.YearlyMaintainenceId = addContactInfoRequest.YearlyMaintenanceId;
                    contactInfo.Location = addContactInfoRequest.Location;
                    contactInfo.Address = addContactInfoRequest.Address;
                    contactInfo.City = addContactInfoRequest.City;
                    contactInfo.State = addContactInfoRequest.State;
                    contactInfo.ZipCode = addContactInfoRequest.Zipcode;
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
              
        public MainResponse GetAllScan(GetScanRequest getScanRequest)
        {
            var getAllScan = _scan.GetAllScan(getScanRequest);

            if (getAllScan.getScans!=null)
            {
                _mainResponse.GetAllScan = getAllScan;
                _mainResponse.Success = true;

            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }

            return _mainResponse;
        }

        public MainResponse AddUpdateYearlyStatementText(AddStatementTextRequest addStatementTextRequest, string actionBy)
        {
            if (addStatementTextRequest.YearlyStatementTextId==0)
            {
                var addStatementText = new YearlyStatementText
                {
                    YearlyMaintenanceId = addStatementTextRequest.YearlyMaintenanceId,
                    StatementName = addStatementTextRequest.StatementName,
                    StatementNumber = addStatementTextRequest.StatementNumber.ToLower(),
                    StatementText = addStatementTextRequest.StatementText,
                    Incentive=addStatementTextRequest.Incentive,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };

                int newId=_yearlyStatementTextRepository.Add(addStatementText).YearlyStatementTextId;
                _mainResponse.Success = true;
                _mainResponse.NewId = newId;
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
            }
            else
            {
                var updateStatementText = _yearlyStatementTextRepository.GetSingle(x => x.YearlyStatementTextId == addStatementTextRequest.YearlyStatementTextId);

                if (updateStatementText!=null)
                {
                    updateStatementText.StatementName = addStatementTextRequest.StatementName;
                    updateStatementText.StatementNumber = addStatementTextRequest.StatementNumber.ToLower();
                    updateStatementText.StatementText = addStatementTextRequest.StatementText;
                    updateStatementText.Incentive = addStatementTextRequest.Incentive;
                    updateStatementText.ModifiedBy = actionBy;
                    updateStatementText.ModifiedDate = DateTime.Now;

                    _yearlyStatementTextRepository.Update(updateStatementText);
                    _mainResponse.Success = true;
                    _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
                }
                else
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
                }
            }

            return _mainResponse;
            
        }

        public MainResponse GetYearlyStatementText(int yearlyMaintenanceId)
        {
            var getStatementText = _yearlyStatementTextRepository.GetYearlyStatementText(yearlyMaintenanceId);

            if (getStatementText.getStatementTexts!=null)
            {
                _mainResponse.GetAllStatementText = getStatementText;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }

            return _mainResponse;
        }

        public MainResponse AddAndUpdateSponsorIncentive(AddSponsorIncentiveRequest addSponsorIncentiveRequest, string actionBy)
        {
            if (addSponsorIncentiveRequest.SponsorIncentiveId==0)
            {
                var addIncentive = new SponsorIncentives
                {
                    YearlyMaintenanceId=addSponsorIncentiveRequest.YearlyMaintenanceId,
                    SponsorAmount = addSponsorIncentiveRequest.Amount,
                    Award=addSponsorIncentiveRequest.Award,
                    IsActive=true,
                    IsDeleted=false,
                    CreatedBy=actionBy,
                    CreatedDate=DateTime.Now
                };

                _sponsorIncentiveRepository.Add(addIncentive);
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
            }
            else
            {
                var updateIncentive = _sponsorIncentiveRepository.GetSingle(x => x.SponsorIncentiveId == 
                                      addSponsorIncentiveRequest.SponsorIncentiveId);

                if (updateIncentive!=null)
                {
                    updateIncentive.SponsorAmount = addSponsorIncentiveRequest.Amount;
                    updateIncentive.Award = addSponsorIncentiveRequest.Award;
                    updateIncentive.ModifiedBy = actionBy;
                    updateIncentive.ModifiedDate = DateTime.Now;

                    _sponsorIncentiveRepository.Update(updateIncentive);
                    _mainResponse.Success = true;
                    _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
                }
                else
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.RECORD_UPDATE_FAILED;
                }
                
            }

            return _mainResponse;
        }

        public MainResponse GetSponsorIncentive(int yearlyMaintenanceId)
        {
            var sponsorIncentives = _sponsorIncentiveRepository.GetAll(x => x.YearlyMaintenanceId == yearlyMaintenanceId && 
                                    x.IsActive == true && x.IsDeleted == false);

            if (sponsorIncentives.Count()!=0)
            {
                GetSponsorAllIncentives getSponsorAllIncentives = new GetSponsorAllIncentives();
                getSponsorAllIncentives.getSponsorIncentives = _mapper.Map<List<GetSponsorIncentives>>(sponsorIncentives);
                _mainResponse.GetSponsorAllIncentives = getSponsorAllIncentives;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }

        public MainResponse DeleteSponsorIncentive(int sponsorIncentiveId, string actionBy)
        {
            var getSponsorIncentive = _sponsorIncentiveRepository.GetSingle(x => x.SponsorIncentiveId == sponsorIncentiveId);

            if (getSponsorIncentive!=null)
            {
                getSponsorIncentive.IsDeleted = true;
                getSponsorIncentive.DeletedBy = actionBy;
                getSponsorIncentive.DeletedDate = DateTime.Now;

                _sponsorIncentiveRepository.Update(getSponsorIncentive);
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_DELETE_SUCCESS;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_Exist_WITH_ID;
            }

            return _mainResponse;
        }

        public MainResponse ActiveInActiveUser(ActiveInActiveRequest activeInActiveRequest)
        {
            var user = _userRepository.GetSingle(x => x.UserId == activeInActiveRequest.UserId);

            if (user!=null)
            {
                user.IsActive = activeInActiveRequest.IsActive;
                _userRepository.Update(user);

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
            }

            return _mainResponse;
        }

        public MainResponse ActiveInActiveGeneralFee(ActiveInActiveGeneralFeeRequest activeInActiveGeneralFeeRequest)
        {
            var feeId = _yearlyMaintenanceFeeRepository.GetSingle(x => x.YearlyMaintainenceFeeId == 
                        activeInActiveGeneralFeeRequest.YearlyMaintenanceFeeId);

            if (feeId!=null)
            {
                feeId.IsActive = activeInActiveGeneralFeeRequest.IsActive;
                _yearlyMaintenanceFeeRepository.Update(feeId);
               
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
            }

            return _mainResponse;
        }

        public MainResponse ActiveInActiveAdFee(ActiveInActiveAdFeeRequest activeInActiveAdFeeRequest)
        {
            var adFee = _yearlyMaintenanceFeeRepository.GetSingle(x => x.YearlyMaintainenceFeeId == activeInActiveAdFeeRequest.YearlyMaintenanceFeeId);

            if (adFee!=null)
            {
                adFee.IsActive = activeInActiveAdFeeRequest.IsActive;
                _yearlyMaintenanceFeeRepository.Update(adFee);
               
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
            }

            return _mainResponse;
        }

        public MainResponse ActiveInActiveClassCategory(ActiveInActiveClassCategory activeInActiveClassCategory)
        {
            var categoryId = _globalCodeRepository.GetSingle(x => x.GlobalCodeId == activeInActiveClassCategory.GlobalCodeId);

            if (categoryId!=null)
            {
                categoryId.IsActive = activeInActiveClassCategory.IsActive;
                _globalCodeRepository.Update(categoryId);

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
            }

            return _mainResponse;
        }

        public MainResponse ActiveInActiveScratchRefund(ActiveInActiveScratchRefund activeInActiveScratchRefund)
        {
            var refund = _refundRepository.GetSingle(x => x.RefundDetailId == activeInActiveScratchRefund.RefundDetailId);

            if (refund!=null)
            {
                refund.IsActive = activeInActiveScratchRefund.IsActive;
                _refundRepository.Update(refund);

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
            }

            return _mainResponse;
        }

        public MainResponse YearlyDataForNextYear(int yearlyMaintenanceId,string actionBy)
        {
            int currentYear = DateTime.Now.Year;
            int nextYear = currentYear + 1;       
            var currentYearId = _yearlyMaintenanceRepository.GetSingle(x => x.YearlyMaintainenceId == yearlyMaintenanceId && x.IsDeleted == false);
            if (currentYear!=currentYearId.Years)
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.SELECT_CURRENT_YEAR;
                return _mainResponse;
            }
            var currentYearFee = _yearlyMaintenanceFeeRepository.GetAll(x => x.YearlyMaintainenceId == currentYearId.YearlyMaintainenceId && x.IsDeleted == false);
            var checkYear = _yearlyMaintenanceRepository.GetSingle(x => x.Years == nextYear && x.IsDeleted == false);
            if (checkYear != null)
            {
                _yearlyMaintenanceRepository.DeleteYearlyFee(checkYear.YearlyMaintainenceId);
                foreach (var fee in currentYearFee)
                {
                    var add = new YearlyMaintainenceFee
                    {
                        YearlyMaintainenceId = checkYear.YearlyMaintainenceId,
                        FeeType = fee.FeeType,
                        FeeName = fee.FeeName,
                        TimeFrame = fee.TimeFrame,
                        Amount = fee.Amount,
                        RefundPercentage = 40,
                        IsActive = fee.IsActive,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                        CreatedBy = actionBy
                    };
                    _yearlyMaintenanceFeeRepository.Add(add);
                }

                var refundDetail = _refundRepository.GetAll(x => x.YearlyMaintenanceId == currentYearId.YearlyMaintainenceId && x.IsDeleted == false);

                foreach (var refund in refundDetail)
                {
                    var refundNext = new RefundDetail
                    {
                        YearlyMaintenanceId = checkYear.YearlyMaintainenceId,
                        DateAfter = refund.DateAfter,
                        DateBefore = refund.DateBefore,
                        FeeTypeId = refund.FeeTypeId,
                        RefundPercentage = refund.RefundPercentage,
                        IsActive = refund.IsActive,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                        CreatedBy = actionBy

                    };

                    _refundRepository.Add(refundNext);
                }

                var getIncentive = _sponsorIncentiveRepository.GetAll(x => x.YearlyMaintenanceId == currentYearId.YearlyMaintainenceId && x.IsDeleted == false);

                foreach (var incentives in getIncentive)
                {
                    var incentive = new SponsorIncentives
                    {
                        YearlyMaintenanceId = checkYear.YearlyMaintainenceId,
                        Award = incentives.Award,
                        SponsorAmount = incentives.SponsorAmount,
                        IsActive = incentives.IsActive,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                        CreatedBy = actionBy
                    };

                    _sponsorIncentiveRepository.Add(incentive);
                }

                var getStatementText = _yearlyStatementTextRepository.GetAll(x => x.YearlyMaintenanceId == currentYearId.YearlyMaintainenceId && x.IsDeleted == false);

                foreach (var text in getStatementText)
                {
                    var addText = new YearlyStatementText
                    {
                        YearlyMaintenanceId= checkYear.YearlyMaintainenceId,
                        StatementName=text.StatementName,
                        StatementNumber=text.StatementNumber,
                        StatementText=text.StatementText,
                        Incentive=text.Incentive,
                        IsActive=text.IsActive,
                        IsDeleted=false,
                        CreatedDate=DateTime.Now,
                        CreatedBy=actionBy
                    };

                    _yearlyStatementTextRepository.Add(addText);
                }
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_YEAR;
            }

            return _mainResponse;

        }
    }
}
