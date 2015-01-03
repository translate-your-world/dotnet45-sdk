using System;
namespace TYW.SDK
{
    public class TywiException : Exception
    {
        public TywiException() : base() { }
        public TywiException(string message) : base(message) { }
        public TywiException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class AccessDeniedException : TywiException
    {
        public AccessDeniedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
