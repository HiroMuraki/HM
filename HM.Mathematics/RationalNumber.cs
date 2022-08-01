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
        /// 实数值
        /// </summary>
        public double RealValue => (double)_numerator / _denominator;
        /// <summary>
        /// 表示分子值
        /// </summary>
        public int Numerator => _numerator;
        /// <summary>
        /// 表示分母值
        /// </summary>
        public int Denominator => _denominator;
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
            _numerator = numerator;
            _denominator = denominator;
            if (_denominator < 0)
            {
                _numerator *= -1;
                _denominator *= -1;
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

            _numerator = numerator;
            _denominator = denominator;
            if (_denominator < 0)
            {
                _numerator *= -1;
                _denominator *= -1;
            }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取化简结果
        /// </summary>
        public RationalNumber GetSimpified()
        {
            int gcd = GetGCD(_numerator, _denominator);
            return new RationalNumber(_numerator / gcd, _denominator / gcd);
        }
        /// <summary>
        /// 获取倒数
        /// </summary>
        /// <returns></returns>
        public RationalNumber GetReciporcal()
        {
            return new RationalNumber(_denominator, _numerator);
        }
        #endregion

        #region 算术运算符重载
        public static RationalNumber operator -(RationalNumber x)
        {
            int numerator = -x._numerator;
            int denominator = x._denominator;
            return new RationalNumber(numerator, denominator);
        }
        public static RationalNumber operator +(RationalNumber x, RationalNumber y)
        {
            int numerator = x._numerator * y._denominator + y._numerator * x._denominator;
            int denominator = x._denominator * y._denominator;
            return new RationalNumber(numerator, denominator);
        }
        public static RationalNumber operator -(RationalNumber x, RationalNumber y)
        {
            return x + (-y);
        }
        public static RationalNumber operator *(RationalNumber x, RationalNumber y)
        {
            int numerator = x._numerator * y._numerator;
            int denominator = x._denominator * y._denominator;
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
            int a = x._numerator * y._denominator;
            int b = y._numerator * x._denominator;
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
            int a = x._numerator * y._denominator;
            int b = y._numerator * x._denominator;
            return a <= b;
        }
        public static bool operator >=(RationalNumber x, RationalNumber y)
        {
            int a = x._numerator * y._denominator;
            int b = y._numerator * x._denominator;
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
            return value.RealValue;
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
                "v" or "value" => RealValue.ToString(),
                _ => ToString()
            };
        }
        public override string ToString()
        {
            if (_denominator == 1)
            {
                return $"{_numerator}";
            }
            if (_numerator == 0)
            {
                return $"0";
            }
            if (_numerator == _denominator)
            {
                return $"1";
            }
            return $"{_numerator}/{_denominator}";
        }
        public override int GetHashCode()
        {
            return _numerator ^ _denominator + _numerator << 2;
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

        private readonly int _numerator;
        private readonly int _denominator;
    }
}
