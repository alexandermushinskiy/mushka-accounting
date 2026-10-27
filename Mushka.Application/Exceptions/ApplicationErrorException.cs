using System;

namespace Mushka.Application.Exceptions
{
    public abstract class ApplicationErrorException : Exception
    {
        protected ApplicationErrorException(string errorKey)
            : base(errorKey)
        {
            ErrorKey = errorKey;
        }

        public string ErrorKey { get; }
    }
}
