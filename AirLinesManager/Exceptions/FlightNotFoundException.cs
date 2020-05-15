using System;
using System.Runtime.Serialization;

namespace AirLinesManager
{
    [Serializable]
    public class FlightNotFoundException : AirlinesManagerException
    {
        public FlightNotFoundException()
        {
        }

        public FlightNotFoundException(string message) : base(message)
        {
        }

        public FlightNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FlightNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}