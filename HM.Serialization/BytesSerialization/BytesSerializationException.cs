using System.Runtime.Serialization;

namespace HM.Serialization.BytesSerialization
{
    [Serializable]
    public class BytesSerializationException : Exception
    {
        public BytesSerializationException() { }
        public BytesSerializationException(string message) : base(message) { }
        public BytesSerializationException(string message, Exception inner) : base(message, inner) { }
        protected BytesSerializationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
