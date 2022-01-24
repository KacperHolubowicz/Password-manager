using System;

namespace Repository.Exceptions
{
    public class NonUniqueException : Exception
    {
        public NonUniqueFlag ExceptionFlag { get; }
        public NonUniqueException(string message, NonUniqueFlag flag) : base(message) 
        {
            ExceptionFlag = flag;
        }
    }

    public enum NonUniqueFlag
    {
        Email,
        Username,
        Login
    }
}