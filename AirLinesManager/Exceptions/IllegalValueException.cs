using System;
using System.Runtime.Serialization;

namespace AirLinesManager
{
    [Serializable]
    public class IllegalValueException : AirlinesManagerException
    {
        public IllegalValueException()
        {
        }

        public IllegalValueException(string message) : base(message)
        {
        }

        public IllegalValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}