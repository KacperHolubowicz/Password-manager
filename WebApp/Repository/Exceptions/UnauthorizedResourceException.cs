using System;

namespace Repository.Exceptions
{
    public class UnauthorizedResourceException : Exception
    {
        public UnauthorizedResourceException(string message) : base(message) 
        {
        }
    }
}