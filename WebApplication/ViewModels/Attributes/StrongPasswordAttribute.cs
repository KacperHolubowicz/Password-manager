using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels.Attributes
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        //TODO
        public StrongPasswordAttribute(int minLength, int maxLength,
            bool requireUppercase, bool requireDigit, bool requireSpecialChar)
        {

        }
    }
}
