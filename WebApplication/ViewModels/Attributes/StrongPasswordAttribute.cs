using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApplication.ViewModels.Attributes
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        public StrongPasswordAttribute(int minLength, int maxLength)
        {
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public int MinLength { get; }
        public int MaxLength { get; }

        public string GetErrorMessage()
        {
            return $"Your password is not strong enough. It must contain at least {MinLength} " +
                $"characters (with a maximum of {MaxLength} characters. It also requires at least one " +
                $"uppercase letter, a digit and a special character.";
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            string providedPassword = (string)value;
            int passwordLength = providedPassword.Length;
            if(passwordLength >= MinLength && passwordLength <= MaxLength &&
                providedPassword.Any(char.IsUpper) && providedPassword.Any(char.IsDigit)
                && providedPassword.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                return ValidationResult.Success; 
            } else
            {
                return new ValidationResult(GetErrorMessage());
            }
        }
    }
}
