using System;
using System.Runtime.Serialization;

namespace AirLinesManager
{
    [Serializable]
    public class UserAccessabillityException : AirlinesManagerException
    {
        public UserAccessabillityException()
        {
        }

        public UserAccessabillityException(string message) : base(message)
        {
        }

        public UserAccessabillityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserAccessabillityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}