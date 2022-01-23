using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.ViewModels.User;

namespace WebApplication.ViewModels.Attributes
{
    public class PasswordConfirmationAttribute : ValidationAttribute
    {
        public string GetErrorMessage()
        {
            return "Password confirmation does not match provided password";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string passwordConfirmation = (string)value;
            UserRegisterVM userVM = (UserRegisterVM)validationContext.ObjectInstance;
            var validation = passwordConfirmation == userVM.Password
                ? ValidationResult.Success : new ValidationResult(GetErrorMessage());

            return validation;
        }
    }
}
