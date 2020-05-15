using System;
using System.Runtime.Serialization;

namespace AirLinesManager
{
    [Serializable]
    public class AirlineNotFoundException : AirlinesManagerException
    {
        public AirlineNotFoundException()
        {
        }

        public AirlineNotFoundException(string message) : base(message)
        {
        }

        public AirlineNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AirlineNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}