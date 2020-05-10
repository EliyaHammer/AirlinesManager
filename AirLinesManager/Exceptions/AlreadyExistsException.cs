using System;
using System.Runtime.Serialization;

namespace AirLinesManager
{
    [Serializable]
    public class AlreadyExistsException : ApplicationException
    {
        public AlreadyExistsException()
        {
        }

        public AlreadyExistsException(string message) : base(message)
        {
        }

        public AlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}