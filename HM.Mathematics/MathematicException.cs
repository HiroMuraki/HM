using System.Runtime.Serialization;

namespace HM.Mathematics
{
    [Serializable]
    public class MathematicException : Exception
    {
        public MathematicException() { }
        public MathematicException(string message) : base(message) { }
        public MathematicException(string message, Exception inner) : base(message, inner) { }

        protected MathematicException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
