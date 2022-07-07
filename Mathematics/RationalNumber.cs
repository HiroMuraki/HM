namespace HM.Mathematics
{
    public readonly struct RationalNumber :
        IFormattable,
        IEquatable<RationalNumber>,
        IComparable<RationalNumber>
    {
        public static readonly RationalNumber One = new(1); //用于表示1
        public static readonly RationalNumber Zero = new(0); //用于表示0
        public static readonly RationalNumber NegativeOne = new(-1); //表示负数

        #region 属性
        /// <summary>
        /// 表示分子值
        /// </summary>
        public int Numerator { get; }
        /// <summary>
        /// 表示分母值
        /// </summary>
        public int Denominator { get; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 创建一个分子为numerator，分母为denominator的分数
        /// </summary>
        /// <param name="numerator">分子</param>
        /// <param name="denominator">分母</param>
        public RationalNumber(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new MathematicException("分母不能为0");
            }
            Numerator = numerator;
            Denominator = denominator;
            if (Denominator < 0)
            {
                Numerator *= -1;
                Denominator *= -1;
            }
        }
        /// <summary>
        /// 创建一个分子为value，分母为1的分数
        /// </summary>
        /// <param name="value">分子值</param>
        public RationalNumber(int value) : this(value, 1) { }
        /// <summary>
        /// 以一个以x/y表示分数的字符串创建分数
        /// </summary>
        /// <param name="value">x/y格式表示的分数</param>
        public RationalNumber(string value)
        {
            int numerator;
            int denominator;
            //如果为/x形式，则转化为1/x
            if (value[0] == '/')
            {
                value = $"1{value}";
            }
            //解析数字字符串
            string[] numbers = value.Split('/');

            //将第一个元素设为分子
            numerator = Convert.ToInt32(numbers[0]);
            //如有第二个元素，则将第二个元素设为分母
            //否则将分母设为1
            if (numbers.Length >= 2)
            {
                denominator = Convert.ToInt32(numbers[1]);
            }
            else
            {
                denominator = 1;
            }

            //如果分母为0，抛出异常
            if (denominator == 0)
            {
                throw new MathematicException("分母不能为0");
            }

            Numerator = numerator;
            Denominator = denominator;
            if (Denominator < 0)
            {
                Numerator *= -1;
                Denominator *= -1;
            }
        }
        #endregion

        #region 公共方法
        public double GetValue()
        {
            return (double)Numerator / Denominator;
        }
        /// <summary>
        /// 获取化简结果
        /// </summary>
        public RationalNumber GetSimpified()
        {
            int gcd = GetGCD(Numerator, Denominator);
            return new RationalNumber(Numerator / gcd, Denominator / gcd);
        }
        /// <summary>
        /// 获取倒数
        /// </summary>
        /// <returns></returns>
        public RationalNumber GetReciporcal()
        {
            return new RationalNumber(Denominator, Numerator);
        }
        #endregion

        #region 算术运算符重载
        public static RationalNumber operator -(RationalNumber x)
        {
            int numerator = -x.Numerator;
            int denominator = x.Denominator;
            return new RationalNumber(numerator, denominator);
        }
        public static RationalNumber operator +(RationalNumber x, RationalNumber y)
        {
            int numerator = x.Numerator * y.Denominator + y.Numerator * x.Denominator;
            int denominator = x.Denominator * y.Denominator;
            return new RationalNumber(numerator, denominator);
        }
        public static RationalNumber operator -(RationalNumber x, RationalNumber y)
        {
            return x + (-y);
        }
        public static RationalNumber operator *(RationalNumber x, RationalNumber y)
        {
            int numerator = x.Numerator * y.Numerator;
            int denominator = x.Denominator * y.Denominator;
            return new RationalNumber(numerator, denominator);
        }
        public static RationalNumber operator /(RationalNumber x, RationalNumber y)
        {
            return x * y.GetReciporcal();
        }
        #endregion

        #region 逻辑运算符重载
        public static bool operator ==(RationalNumber x, RationalNumber y)
        {
            int a = x.Numerator * y.Denominator;
            int b = y.Numerator * x.Denominator;
            return a == b;
        }
        public static bool operator !=(RationalNumber x, RationalNumber y)
        {
            return !(x == y);
        }
        public static bool operator <(RationalNumber x, RationalNumber y)
        {
            return !(x >= y);
        }
        public static bool operator >(RationalNumber x, RationalNumber y)
        {
            return !(x <= y);
        }
        public static bool operator <=(RationalNumber x, RationalNumber y)
        {
            int a = x.Numerator * y.Denominator;
            int b = y.Numerator * x.Denominator;
            return a <= b;
        }
        public static bool operator >=(RationalNumber x, RationalNumber y)
        {
            int a = x.Numerator * y.Denominator;
            int b = y.Numerator * x.Denominator;
            return a >= b;
        }
        #endregion

        #region 强制转换运算符
        public static implicit operator RationalNumber(int value)
        {
            return new RationalNumber(value, 1);
        }
        public static implicit operator double(RationalNumber value)
        {
            return value.GetValue();
        }
        public static implicit operator RationalNumber(string value)
        {
            return new RationalNumber(value);
        }
        #endregion

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return format?.ToLower() switch
            {
                "s" or "simpified" => GetSimpified().ToString(),
                "v" or "value" => GetValue().ToString(),
                _ => ToString()
            };
        }
        public override string ToString()
        {
            if (Denominator == 1)
            {
                return $"{Numerator}";
            }
            if (Numerator == 0)
            {
                return $"0";
            }
            if (Numerator == Denominator)
            {
                return $"1";
            }
            return $"{Numerator}/{Denominator}";
        }
        public override int GetHashCode()
        {
            return Numerator ^ Denominator + Numerator * Denominator;
        }
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (typeof(RationalNumber) != obj.GetType())
            {
                return false;
            }
            return (RationalNumber)obj == this;
        }
        public bool Equals(RationalNumber other)
        {
            return other == this;
        }
        public int CompareTo(RationalNumber other)
        {
            if (this < other)
            {
                return -1;
            }
            else if (this > other)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public int CompareTo(object? obj)
        {
            if (obj?.GetType() != typeof(RationalNumber))
            {
                return -1;
            }
            return CompareTo((RationalNumber)obj);
        }
        #region 辅助方法
        /// <summary>
        /// 获取两个数的最大公约数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static int GetGCD(int a, int b)
        {
            while (b != 0)
            {
                int t = a % b;
                a = b;
                b = t;
            }
            return a;
        }
        #endregion
    }
}
