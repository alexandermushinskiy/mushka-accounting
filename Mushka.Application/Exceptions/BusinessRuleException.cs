namespace Mushka.Application.Exceptions;

public sealed class BusinessRuleException : ApplicationErrorException
{
    public BusinessRuleException(string errorKey)
        : base(errorKey)
    {
    }
}
