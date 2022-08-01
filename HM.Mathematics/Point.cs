namespace HM.Mathematics
{
    [Serializable]
    public readonly struct Point : IFormattable, IEquatable<Point>
    {
        public double X { get; }
        public double Y { get; }

        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
        public static Point operator *(Point a, double b)
        {
            return new Point(a.X * b, a.Y * b);
        }
        public static Point operator *(double a, Point b)
        {
            return b * a;
        }
        public static Point operator /(Point a, double b)
        {
            return new Point(a.X / b, a.Y / b);
        }
        public static Point operator -(Point point)
        {
            return new Point(-point.X, -point.Y);
        }
        public static Point GetPosition(Point start, double distance, double degrees)
        {
            double rad = degrees / _degreesPerRad;
            double dx = distance * Math.Cos(rad);
            double dy = distance * Math.Sin(rad);

            return new Point(start.X + dx, start.Y + dy);
        }
        public static double Distance(Point a, Point b)
        {
            double dx = a.X - b.X;
            double dy = b.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        public static Point Center(Point a, Point b)
        {
            return new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);
        }

        public void Deconstruct(out double x, out double y)
        {
            x = X;
            y = Y;
        }
        public Point GetPosition(double distance, double degrees)
        {
            return GetPosition(this, distance, degrees);
        }
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (obj.GetType() != typeof(Point))
            {
                return false;
            }
            return Equals((Point)obj);
        }
        public override int GetHashCode()
        {
            return (int)X ^ (int)Y;
        }
        public bool Equals(Point other)
        {
            return this == other;
        }
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return $"({X.ToString(format, formatProvider)}, {Y.ToString(format, formatProvider)})";
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        private static readonly double _degreesPerRad = 180 / Math.PI;
    }
}
