using System.Runtime.Serialization;

namespace HM.CLI
{
    [Serializable]
    public class InvalidLaunchArgumentsOptionException : Exception
    {
        public InvalidLaunchArgumentsOptionException() { }
        public InvalidLaunchArgumentsOptionException(string message) : base(message) { }
        public InvalidLaunchArgumentsOptionException(string message, Exception inner) : base(message, inner) { }
        protected InvalidLaunchArgumentsOptionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}