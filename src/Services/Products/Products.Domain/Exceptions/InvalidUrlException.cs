using System.Runtime.Serialization;

namespace Products.Domain.Exceptions
{
    [Serializable]
    public class InvalidUrlException : Exception
    {
        public InvalidUrlException() : base("Invalid Url")
        {
            
        }

        public InvalidUrlException(string? message) : base(message)
        {
        }

        public InvalidUrlException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidUrlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}