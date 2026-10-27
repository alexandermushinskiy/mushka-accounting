namespace Mushka.Application.Exceptions;

public sealed class NotFoundException : ApplicationErrorException
{
    public NotFoundException(string errorKey)
        : base(errorKey)
    {
    }
}
