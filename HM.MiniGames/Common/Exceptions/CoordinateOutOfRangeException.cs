using System.Runtime.Serialization;
using System;

namespace HM.MiniGames.Common.Exceptions
{
    [Serializable]
    public class CoordinateOutOfRangeException : Exception
    {
        public Coordinate Coordinate { get; }

        public CoordinateOutOfRangeException(Coordinate coordinate) : this($"Coordiante `{coordinate}` is out of layout")
        {
            Coordinate = coordinate;
        }
        public CoordinateOutOfRangeException(string message) : base(message) { }
        public CoordinateOutOfRangeException(string message, Exception inner) : base(message, inner) { }

        protected CoordinateOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}