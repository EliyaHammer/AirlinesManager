using System;
using System.Runtime.Serialization;

namespace AirLinesManager
{
    [Serializable]
    public class WrongPasswordException : AirlinesManagerException
    {
        public WrongPasswordException()
        {
        }

        public WrongPasswordException(string message) : base(message)
        {
        }

        public WrongPasswordException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}