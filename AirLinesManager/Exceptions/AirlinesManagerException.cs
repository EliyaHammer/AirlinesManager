using System;
using System.Runtime.Serialization;

namespace AirLinesManager
{
    [Serializable]
    public class AirlinesManagerException : ApplicationException
    {
        public AirlinesManagerException()
        {
        }

        public AirlinesManagerException(string message) : base(message)
        {
        }

        public AirlinesManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AirlinesManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}