using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Helper;
using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AAYHS.Service.Service
{
    public class UserService:IUserService
    {
        

        #region readonly        
        private readonly IUserRepository _userRepository;
        private readonly IEmailSenderRepository _emailRepository;
        private readonly IApplicationSettingRepository _applicationRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Private
        private MainResponse _mainResponse;
        #endregion

        public UserService(IUserRepository userRepository,IEmailSenderRepository emailRepository,
                          IApplicationSettingRepository applicationRepository,IUserRoleRepository userRoleRepository ,
                          IRoleRepository roleRepository,IMapper Mapper)
        {
           
            _userRepository = userRepository;
            _emailRepository = emailRepository;
            _applicationRepository = applicationRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse LoginUser(UserLoginRequest userLoginRequest)
        {
            string encodePassword = EncryptDecryptHelper.GetMd5Hash(userLoginRequest.Password);
            var userDetails = _userRepository.GetSingle(x => x.UserName == userLoginRequest.UserName.ToLower() && x.Password == encodePassword && x.IsActive ==true && x.IsDeleted == false);
            if (userDetails != null)
            {
                if (userDetails.IsApproved==true)
                {
                    var userRole = _userRoleRepository.GetSingle(x => x.UserId == userDetails.UserId);
                    if (userRole!=null)
                    {
                        var role = _roleRepository.GetSingle(x => x.RoleId == userRole.RoleId);
                        var userResponse = _mapper.Map<UserResponse>(userDetails);
                        userResponse.Role = role.RoleName;
                        _mainResponse.UserResponse = userResponse;

                    }
                    else
                    {
                        var userResponse = _mapper.Map<UserResponse>(userDetails);
                        _mainResponse.UserResponse = userResponse;
                    }
                   
                    _mainResponse.Message = Constants.LOG_IN;
                    _mainResponse.Success = true;
                }
                else
                {
                    _mainResponse.Message = Constants.ADMIN_APPROVAL;
                    _mainResponse.Success = false;
                }
                
            }
            else
            {
                _mainResponse.Message = Constants.USERNAME_PASSWORD_INCORRECT;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public MainResponse CreateNewAccount(CreateNewAccountRequest userRequest)
        {
            string encodedPassword = !string.IsNullOrWhiteSpace(userRequest.Password) ? EncryptDecryptHelper.GetMd5Hash(userRequest.Password) : null;

            var emailExist= _userRepository.GetSingle(x => x.Email == userRequest.Email.ToLower());
            if (emailExist!=null)
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.EMAIL_ALREADY_EXIST;
                return _mainResponse;
            }
            var userDetails = _userRepository.GetSingle(x => x.UserName == userRequest.UserName.ToLower());
            if (userDetails != null)
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.USERNAME_ALREADY;
                return _mainResponse;
            }
            
                var users = new User
                {
                    UserName = userRequest.UserName.ToLower(),
                    FirstName = userRequest.FirstName,
                    LastName = userRequest.LastName,
                    Email = userRequest.Email.ToLower(),
                    Password = encodedPassword,
                    CreatedBy = userRequest.UserName,
                    CreatedDate = DateTime.Now

                };
                    
                _userRepository.Add(users);

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.ACCOUNT_CREATED;


            return _mainResponse;
        }
        public MainResponse ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            var user = _userRepository.GetSingle(x => x.Email == forgotPasswordRequest.Email.ToLower() );
            if (user!=null)
            {
                string guid = Guid.NewGuid().ToString();
                user.ResetToken = guid;
                user.ResetTokenExpired = DateTime.UtcNow.AddMinutes(60);
                 _userRepository.Update(user);

                //get email settings
                var settings = _applicationRepository.GetAll().FirstOrDefault();

                // Send Fortget Password Email
                EmailRequest email = new EmailRequest();
                email.To = forgotPasswordRequest.Email;
                email.SenderEmail = settings.CompanyEmail;
                email.CompanyEmail = settings.CompanyEmail;
                email.CompanyPassword = settings.CompanyPassword;
                email.Url = settings.ResetPasswordUrl;
                email.Token = guid;
                email.TemplateType = "Forgot Password";
                
                _emailRepository.SendEmail(email);
 
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.FORGET_PASSWORD_EMAIL;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }
        public MainResponse ValidateResetPasswordToken(ValidateResetPasswordRequest validateResetPasswordRequest)
        {
           
                var userDetails = _userRepository.GetSingle(x => x.Email == validateResetPasswordRequest.Email.ToLower() && x.ResetToken == validateResetPasswordRequest.Token && x.ResetTokenExpired > DateTime.UtcNow);
            if (userDetails == null)
            {
                _mainResponse.Message = Constants.RESET_LINK_EXPIRED;
                _mainResponse.Success = false;
            }
            else
            {
                _mainResponse.Success = true;
            }
            return _mainResponse;
          
        }
        public MainResponse ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
           
                var user = _userRepository.GetSingle(x => x.Email == changePasswordRequest.Email.ToLower() && x.ResetToken == changePasswordRequest.Token);

                if (user != null)
                {
                     user.Password = EncryptDecryptHelper.GetMd5Hash(changePasswordRequest.NewPassword);
                     user.ResetToken = null;
                     user.ResetTokenExpired = null;
                     user.ModifiedDate = DateTime.Now;
                     user.ModifiedBy = changePasswordRequest.Email;

                     _userRepository.Update(user);
                                  
                    _mainResponse.Success = true;
                    _mainResponse.Message = Constants.PASSWORD_CHANGED;
                   
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
