using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.IService
{
    public interface IUserService
    {
        MainResponse LoginUser(UserLoginRequest userLoginRequest);
        MainResponse CreateNewAccount(CreateNewAccountRequest userRequest);
        MainResponse ForgetPassword(ForgotPasswordRequest forgotPasswordRequest);
        MainResponse ValidateResetPasswordToken(ValidateResetPasswordRequest validateResetPasswordRequest);
        MainResponse ChangePassword(ChangePasswordRequest changePasswordRequest);
    }
}
