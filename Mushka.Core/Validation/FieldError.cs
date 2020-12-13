namespace Mushka.Core.Validation
{
    public class FieldError
    {
        public string ErrorKey { get; }
        public string Message { get; }

        public FieldError(string errorKey)
        {
            ErrorKey = errorKey;
        }

        public FieldError(string errorKey, string message)
            : this(errorKey)
        {
            Message = message;
        }
    }
}