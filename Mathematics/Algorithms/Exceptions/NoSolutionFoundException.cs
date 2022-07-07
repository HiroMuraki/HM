using System.Runtime.Serialization;

namespace HM.Mathematics.Algorithm
{
    [Serializable]
    public class NoSolutionFoundException : Exception
    {
        public NoSolutionFoundException() { }
        public NoSolutionFoundException(string message) : base(message) { }
        public NoSolutionFoundException(string message, Exception inner) : base(message, inner) { }
        protected NoSolutionFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
