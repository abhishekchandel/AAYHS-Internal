using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
    public class UserLoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
    public class CreateNewAccountRequest
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
    public class ForgotPasswordRequest
    {   
        [Required]
        public string Username { get; set; }       
        [Required]
        public string Url { get; set; }
    }
    public class ValidateResetPasswordRequest
    {  
        [Required]
        public string Username { get; set; }        
        [Required]
        public string Token { get; set; }
    }
    public class ChangePasswordRequest 
    {
       
        [Required]
        public string Username { get; set; }
        
        [Required]        
        public string NewPassword { get; set; }

    }
}
