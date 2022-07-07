namespace HM.Serialization.Csv
{
    [Serializable]
    public readonly struct Cell : IConvertible, IEquatable<Cell>
    {
        public bool ToBoolean()
        {
            return ToBoolean(null);
        }
        public bool ToBoolean(IFormatProvider? provider)
        {
            return Convert.ToBoolean(_data, provider);
        }
        public byte ToByte()
        {
            return ToByte(null);
        }
        public byte ToByte(IFormatProvider? provider)
        {
            return Convert.ToByte(_data, provider);
        }
        public char ToChar()
        {
            return ToChar(null);
        }
        public char ToChar(IFormatProvider? provider)
        {
            return Convert.ToChar(_data, provider);
        }
        public DateTime ToDateTime()
        {
            return ToDateTime(null);
        }
        public DateTime ToDateTime(IFormatProvider? provider)
        {
            return Convert.ToDateTime(_data, provider);
        }
        public decimal ToDecimal()
        {
            return ToDecimal(null);
        }
        public decimal ToDecimal(IFormatProvider? provider)
        {
            return Convert.ToDecimal(_data, provider);
        }
        public double ToDouble()
        {
            return ToDouble(null);
        }
        public double ToDouble(IFormatProvider? provider)
        {
            return Convert.ToDouble(_data, provider);
        }
        public short ToInt16()
        {
            return ToInt16(null);
        }
        public short ToInt16(IFormatProvider? provider)
        {
            return Convert.ToInt16(_data, provider);
        }
        public int ToInt32()
        {
            return ToInt32(null);
        }
        public int ToInt32(IFormatProvider? provider)
        {
            return Convert.ToInt32(_data, provider);
        }
        public long ToInt64()
        {
            return ToInt64(null);
        }
        public long ToInt64(IFormatProvider? provider)
        {
            return Convert.ToInt64(_data, provider);
        }
        public sbyte ToSByte()
        {
            return ToSByte(null);
        }
        public sbyte ToSByte(IFormatProvider? provider)
        {
            return Convert.ToSByte(_data, provider);
        }
        public float ToSingle()
        {
            return ToSingle(null);
        }
        public float ToSingle(IFormatProvider? provider)
        {
            return Convert.ToSingle(_data, provider);
        }
        public string ToString(IFormatProvider? provider)
        {
            return Convert.ToString(_data, provider);
        }
        public ushort ToUInt16()
        {
            return ToUInt16(null);
        }
        public ushort ToUInt16(IFormatProvider? provider)
        {
            return Convert.ToUInt16(_data, provider);
        }
        public uint ToUInt32()
        {
            return ToUInt32(null);
        }
        public uint ToUInt32(IFormatProvider? provider)
        {
            return Convert.ToUInt32(_data, provider);
        }
        public ulong ToUInt64()
        {
            return ToUInt64(null);
        }
        public ulong ToUInt64(IFormatProvider? provider)
        {
            return Convert.ToUInt64(_data, provider);
        }

        public TypeCode GetTypeCode()
        {
            return _data.GetTypeCode();
        }
        public object ToType(Type conversionType, IFormatProvider? provider)
        {
            return Type.GetTypeCode(conversionType) switch
            {
                TypeCode.Boolean => ToBoolean(provider),
                TypeCode.Byte => ToByte(provider),
                TypeCode.Char => ToChar(provider),
                TypeCode.DateTime => ToDateTime(provider),
                TypeCode.Decimal => ToDecimal(provider),
                TypeCode.Double => ToDouble(provider),
                TypeCode.Int16 => ToInt16(provider),
                TypeCode.Int32 => ToInt32(provider),
                TypeCode.Int64 => ToInt64(provider),
                TypeCode.SByte => ToSByte(provider),
                TypeCode.Single => ToSingle(provider),
                TypeCode.String => ToString(provider),
                TypeCode.UInt16 => ToUInt16(provider),
                TypeCode.UInt32 => ToUInt32(provider),
                TypeCode.UInt64 => ToUInt64(provider),
                TypeCode.Object => _data,
                TypeCode.Empty => throw new NotSupportedException(),
                TypeCode.DBNull => throw new NotSupportedException(),
                _ => throw new NotSupportedException()
            };
        }
        public override string ToString()
        {
            return _data;
        }
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (obj.GetType() == this.GetType())
            {
                return false;
            }
            return Equals((Cell)obj);
        }
        public bool Equals(Cell other)
        {
            return _data == other._data;
        }
        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        public static bool operator ==(Cell left, Cell right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Cell left, Cell right)
        {
            return !left.Equals(right);
        }
        public static implicit operator Cell(string value)
        {
            return new Cell(value);
        }
        public static implicit operator Cell(short value)
        {
            return new Cell(value.ToString());
        }
        public static implicit operator Cell(ushort value)
        {
            return new Cell(value.ToString());
        }
        public static implicit operator Cell(int value)
        {
            return new Cell(value.ToString());
        }
        public static implicit operator Cell(uint value)
        {
            return new Cell(value.ToString());
        }
        public static implicit operator Cell(long value)
        {
            return new Cell(value.ToString());
        }
        public static implicit operator Cell(ulong value)
        {
            return new Cell(value.ToString());
        }
        public static implicit operator Cell(byte value)
        {
            return new Cell(value.ToString());
        }
        public static implicit operator Cell(sbyte value)
        {
            return new Cell(value.ToString());
        }
        public static implicit operator Cell(float value)
        {
            return new Cell(value.ToString());
        }
        public static implicit operator Cell(double value)
        {
            return new Cell(value.ToString());
        }
        public static implicit operator Cell(char value)
        {
            return new Cell(value.ToString());
        }
        public static implicit operator Cell(decimal value)
        {
            return new Cell(value.ToString());
        }
        public static implicit operator Cell(DateTime value)
        {
            return new Cell(value.ToString());
        }

        public Cell(string data)
        {
            _data = data;
        }

        private readonly string _data;
    }
}
