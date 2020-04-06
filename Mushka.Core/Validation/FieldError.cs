namespace Mushka.Core.Validation
{
    public class FieldError
    {
        public string ErrorKey { get; }

        public FieldError(string errorKey)
        {
            ErrorKey = errorKey;
        }
    }
}