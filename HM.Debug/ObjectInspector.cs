using System.Collections;
using System.Reflection;
using System.Text;

namespace HM.Debug
{
    public enum ByteOrder
    {
        LittleEndian,
        BigEndian
    }

    public enum BitOrder
    {
        LittleEndian,
        BigEndian
    }

    public static class ObjectInspector
    {
        public static readonly BindingFlags BindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static string InspectFields(object obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            var sb = new StringBuilder();
            var fieldInfos = GetFieldInfos(obj.GetType());
            foreach (var fieldInfo in fieldInfos)
            {
                var val = fieldInfo.GetValue(obj);
                if (val is null)
                {
                    sb.AppendLine($"{fieldInfo.Name}: null");
                }
                else
                {
                    if (fieldInfo.FieldType.IsArray)
                    {
                        Array array = (Array)val!;
                        var arrayStr = new StringBuilder();
                        arrayStr.Append("[");
                        for (int i = 0; i < array.Length; i++)
                        {
                            arrayStr.Append(array.GetValue(i) ?? "null");
                            if (i < array.Length - 1)
                            {
                                arrayStr.Append(", ");
                            }
                        }
                        arrayStr.Append("]");
                        sb.AppendLine($"{fieldInfo.Name}: {arrayStr.ToString()}");
                    }
                    else
                    {
                        sb.AppendLine($"{fieldInfo.Name}: {val}");
                    }

                }
            }
            return sb.ToString();
        }
        public static string InspectProperties(object obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            var sb = new StringBuilder();
            var fieldInfos = GetPropertyInfos(obj.GetType());
            foreach (var fieldInfo in fieldInfos)
            {
                var val = fieldInfo.GetValue(obj);
                if (val is null)
                {
                    sb.AppendLine($"{fieldInfo.Name}: null");
                }
                else
                {
                    if (fieldInfo.PropertyType.IsArray)
                    {
                        Array array = (Array)val!;
                        var arrayStr = new StringBuilder();
                        arrayStr.Append("[");
                        for (int i = 0; i < array.Length; i++)
                        {
                            arrayStr.Append(array.GetValue(i) ?? "null");
                            if (i < array.Length - 1)
                            {
                                arrayStr.Append(", ");
                            }
                        }
                        arrayStr.Append("]");
                        sb.AppendLine($"{fieldInfo.Name}: {arrayStr.ToString()}");
                    }
                    else
                    {
                        sb.AppendLine($"{fieldInfo.Name}: {val}");
                    }

                }
            }
            return sb.ToString();
        }
        public static bool AreFieldsEqual(object a, object b)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);

            return TestFieldsEquality(a, b);
        }
        public static bool ArePropertiesEqual(object a, object b)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);

            return TestPropertiesEquality(a, b);
        }
        public static string To8bitsString(object value)
        {
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte: return ByteToBitString((byte)value!);
                case TypeCode.Int16: return BytesToString(BitConverter.GetBytes((short)value!));
                case TypeCode.UInt16: return BytesToString(BitConverter.GetBytes((ushort)value!));
                case TypeCode.Int32: return BytesToString(BitConverter.GetBytes((int)value!));
                case TypeCode.UInt32: return BytesToString(BitConverter.GetBytes((uint)value!));
                case TypeCode.Int64: return BytesToString(BitConverter.GetBytes((long)value!));
                case TypeCode.UInt64: return BytesToString(BitConverter.GetBytes((ulong)value!));
                case TypeCode.Single: return BytesToString(BitConverter.GetBytes((float)value!));
                case TypeCode.Double: return BytesToString(BitConverter.GetBytes((double)value!));
                case TypeCode.Decimal: return String.Join(' ', from intVal in decimal.GetBits((decimal)value) select To8bitsString(intVal));
                case TypeCode.Boolean: return BytesToString(BitConverter.GetBytes((bool)value!));
                case TypeCode.Char: return BytesToString(BitConverter.GetBytes((char)value!));
                case TypeCode.DateTime: return BytesToString(BitConverter.GetBytes(((DateTime)value).Ticks));
                default: throw new ArgumentException();
            }
        }


        private static readonly Dictionary<Guid, FieldInfo[]> _fieldInfosCache = new();
        private static readonly Dictionary<Guid, PropertyInfo[]> _propertyInfosCache = new();
        private static FieldInfo[] GetFieldInfos(Type type)
        {
            if (_fieldInfosCache.TryGetValue(type.GUID, out var reuslt))
            {
                return reuslt;
            }
            else
            {
                reuslt = type.GetFields(BindingFlags);
                _fieldInfosCache[type.GUID] = reuslt;
                return reuslt;
            }
        }
        private static PropertyInfo[] GetPropertyInfos(Type type)
        {
            if (_propertyInfosCache.TryGetValue(type.GUID, out var reuslt))
            {
                return reuslt;
            }
            else
            {
                reuslt = type.GetProperties(BindingFlags);
                _propertyInfosCache[type.GUID] = reuslt;
                return reuslt;
            }
        }
        private static string BytesToString(byte[] bytes, char delimiter = '-', ByteOrder byteOrder = ByteOrder.BigEndian, BitOrder bitOrder = BitOrder.BigEndian)
        {
            var bitArray = new BitArray(bytes);
            string[] byteStrArray = new string[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                int index = i;
                switch (byteOrder)
                {
                    case ByteOrder.LittleEndian:
                        index = i;
                        break;
                    case ByteOrder.BigEndian:
                        index = bytes.Length - 1 - i;
                        break;
                }
                byteStrArray[index] = ByteToBitString(bytes[i], bitOrder);
            }
            return String.Join(delimiter, byteStrArray);
        }
        private static string ByteToBitString(byte byteVal, BitOrder bitOrder = BitOrder.BigEndian)
        {
            char[] bitChars = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
            int offset = 0;
            int shifted = byteVal;
            while (shifted != 0)
            {
                switch (bitOrder)
                {
                    case BitOrder.LittleEndian:
                        bitChars[offset] = (shifted & 1) == 0 ? '0' : '1';
                        break;
                    case BitOrder.BigEndian:
                        bitChars[bitChars.Length - 1 - offset] = (shifted & 1) == 0 ? '0' : '1';
                        break;
                }
                shifted = shifted >> 1;
                ++offset;
            }
            return new string(bitChars);
        }
        private static bool TestFieldsEquality(object? a, object? b)
        {
            if (a is null && b is null) return true;
            if (a is null && b is not null) return false;
            if (a is not null && b is null) return false;
            if (a!.GetType() != b!.GetType()) return false;

            var dataType = a.GetType();
            switch (Type.GetTypeCode(dataType))
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    return a.Equals(b);
                default: break;
            }

            if (dataType.IsArray)
            {
                var arrayA = (Array)a;
                var arrayB = (Array)b;
                if (arrayA.Length != arrayB.Length) return false;

                for (int i = 0; i < arrayA.Length; i++)
                {
                    if (!TestFieldsEquality(arrayA.GetValue(i), arrayB.GetValue(i)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var fieldInfo in GetFieldInfos(dataType))
                {
                    object? valueOfA = fieldInfo.GetValue(a);
                    object? valueOfB = fieldInfo.GetValue(b);
                    if (!TestFieldsEquality(valueOfA, valueOfB))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        private static bool TestPropertiesEquality(object? a, object? b)
        {
            if (a is null && b is null) return true;
            if (a is null && b is not null) return false;
            if (a is not null && b is null) return false;
            if (a!.GetType() != b!.GetType()) return false;

            var dataType = a.GetType();
            switch (Type.GetTypeCode(dataType))
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    return a.Equals(b);
                default: break;
            }

            if (dataType.IsArray)
            {
                var arrayA = (Array)a;
                var arrayB = (Array)b;
                if (arrayA.Length != arrayB.Length) return false;

                for (int i = 0; i < arrayA.Length; i++)
                {
                    if (!TestPropertiesEquality(arrayA.GetValue(i), arrayB.GetValue(i)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var fieldInfo in GetPropertyInfos(dataType))
                {
                    object? valueOfA = fieldInfo.GetValue(a);
                    object? valueOfB = fieldInfo.GetValue(b);
                    if (!TestPropertiesEquality(valueOfA, valueOfB))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
