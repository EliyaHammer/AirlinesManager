using System;
using System.Runtime.Serialization;

namespace AirLinesManager
{
    [Serializable]
    public class WrongUsernameException : AirlinesManagerException
    {
        public WrongUsernameException()
        {
        }

        public WrongUsernameException(string message) : base(message)
        {
        }

        public WrongUsernameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongUsernameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}