
namespace FxNet.Test.Domain.Exceptions
{
    public class SecureValidationException : SecureException
    {
        public SecureValidationException(string message) : base(message) { }
    }
}
