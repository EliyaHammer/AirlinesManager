using System;
using System.Runtime.Serialization;

namespace AirLinesManager
{
    [Serializable]
    public class UserNotFoundException : AirlinesManagerException
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }

        public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}