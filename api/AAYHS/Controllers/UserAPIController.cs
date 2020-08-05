using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Helper;
using AAYHS.Core.Shared.Static;
using AAYHS.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AAYHS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        #region readonly
        private IOptions<AppSettings> _settings;
        private readonly IUserService _userService;
        private MainResponse _mainResponse;
        #endregion

        #region private
        private string _jsonString = string.Empty;
        #endregion

        #region JWT Functions
        private string GenerateJSONWebToken(int userId, string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettingConfigurations.AppSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.NameId, Convert.ToString(userId)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            var token = new JwtSecurityToken(AppSettingConfigurations.AppSettings.ValidIssuer,
                                            AppSettingConfigurations.AppSettings.ValidAudience,
                                            claims,
                                            expires: DateTime.Now.AddMinutes(Convert.ToInt32(AppSettingConfigurations.AppSettings.Timeout)),
                                            signingCredentials: credentials
                                            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        public UserAPIController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// This api used for login the user
        /// </summary>
        /// <param name="userLoginRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult LoginUser(UserLoginRequest userLoginRequest)
        {
            _mainResponse = _userService.LoginUser(userLoginRequest);
            if (_mainResponse.Success==true)
            {
                _mainResponse.UserResponse.Token= GenerateJSONWebToken(_mainResponse.UserResponse.UserId, _mainResponse.UserResponse.UserName);
            }
            _jsonString = Mapper.Convert<UserResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used fot create new user account
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateNewAccount(CreateNewAccountRequest userRequest)
        {
            _mainResponse = _userService.CreateNewAccount(userRequest);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to send the email for forget password
        /// </summary>
        /// <param name="forgotPasswordRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ForgetPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            _mainResponse = _userService.ForgetPassword(forgotPasswordRequest);
            if (_mainResponse.Success == true)
            {
                _mainResponse.UserResponse.Token = GenerateJSONWebToken(_mainResponse.UserResponse.UserId, _mainResponse.UserResponse.UserName);
            }
            _jsonString = Mapper.Convert<UserResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to validate the reset password token
        /// </summary>
        /// <param name="forgotPasswordRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ValidateResetPasswordToken(ValidateResetPasswordRequest validateResetPasswordRequest)
        {
            _mainResponse = _userService.ValidateResetPasswordToken(validateResetPasswordRequest);
            if (_mainResponse.Success == true)
            {
                _mainResponse.UserResponse.Token = GenerateJSONWebToken(_mainResponse.UserResponse.UserId, _mainResponse.UserResponse.UserName);
            }
            _jsonString = Mapper.Convert<UserResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to change the password
        /// </summary>
        /// <param name="changePasswordRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            _mainResponse = _userService.ChangePassword(changePasswordRequest);
            _jsonString = Mapper.Convert<UserResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }
}
