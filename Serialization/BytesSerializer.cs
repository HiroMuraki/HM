#pragma warning disable IDE0049
using System.Reflection;
using System.Text;

namespace HM.Serialization
{
    public class BytesSerializer
    {
        public static BytesSerializer Default { get; } = new();
        public Encoding TextEncoding { get; init; } = Encoding.UTF8;

        public byte[] SerializeToBytes(object obj)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(obj);

                var objType = obj.GetType();

                switch (Type.GetTypeCode(objType))
                {
                    case TypeCode.Empty: throw new NullReferenceException("Unable to serialize null value");
                    case TypeCode.SByte: return new Byte[] { (Byte)(SByte)obj };
                    case TypeCode.Byte: return new Byte[] { (Byte)obj };
                    case TypeCode.Int16: return BitConverter.GetBytes((Int16)obj!);
                    case TypeCode.UInt16: return BitConverter.GetBytes((UInt16)obj!);
                    case TypeCode.Int32: return BitConverter.GetBytes((Int32)obj!);
                    case TypeCode.UInt32: return BitConverter.GetBytes((UInt32)obj!);
                    case TypeCode.Int64: return BitConverter.GetBytes((Int64)obj!);
                    case TypeCode.UInt64: return BitConverter.GetBytes((UInt64)obj!);
                    case TypeCode.Single: return BitConverter.GetBytes((Single)obj!);
                    case TypeCode.Double: return BitConverter.GetBytes((Double)obj!);
                    case TypeCode.Decimal:
                        int[] intArray = decimal.GetBits((Decimal)obj!);
                        byte[] decimalBytes = new byte[sizeof(Decimal)];
                        for (int i = 0; i < intArray.Length; i++)
                        {
                            Array.Copy(BitConverter.GetBytes(intArray[i]), 0, decimalBytes, sizeof(Int32) * i, sizeof(Int32));
                        }
                        return decimalBytes;
                    case TypeCode.Boolean: return BitConverter.GetBytes((Boolean)obj!);
                    case TypeCode.Char: return BitConverter.GetBytes((Char)obj!);
                    case TypeCode.DateTime: return BitConverter.GetBytes(((DateTime)obj!).Ticks);
                    case TypeCode.String:
                        // 编码字符串，写入编码后的字节数组长度 + 字节数组长度
                        byte[] encoded = TextEncoding.GetBytes((string)obj!);
                        byte[] strLengthInfo = BitConverter.GetBytes(encoded.Length);
                        byte[] result = new byte[sizeof(Int32) + encoded.Length];
                        Array.Copy(strLengthInfo, 0, result, 0, strLengthInfo.Length);
                        Array.Copy(encoded, 0, result, sizeof(Int32), encoded.Length);
                        return result;
                    case TypeCode.Object:
                        if (objType.IsArray)
                        {
                            var array = (Array)obj;
                            if (array.Rank > 1)
                            {
                                throw new ArgumentException("Unable to serialize multi-dimensional array, convert it to one-dimensional instead");
                            }
                            byte[] lengthInfo = BitConverter.GetBytes(array.Length);
                            var objBytes = new List<byte>();
                            for (Int32 i = 0; i < array.Length; i++)
                            {
                                var element = array.GetValue(i) ?? throw new NullReferenceException("Element in array can't be null");
                                objBytes.AddRange(SerializeToBytes(element));
                            }

                            byte[] arrayResult = new byte[sizeof(Int32) + objBytes.Count];
                            Array.Copy(lengthInfo, 0, arrayResult, 0, lengthInfo.Length);
                            Array.Copy(objBytes.ToArray(), 0, arrayResult, sizeof(Int32), objBytes.Count);
                            return arrayResult.ToArray();
                        }
                        else if (objType.IsDefined(typeof(BytesSerializableAttribute)))
                        {
                            var bytes = new List<byte>();
                            foreach (var fieldInfo in GetFieldInfos(obj.GetType()))
                            {
                                var value = fieldInfo.GetValue(obj) ?? throw new NullReferenceException($"Value of '{fieldInfo}' cant't be null");
                                bytes.AddRange(SerializeToBytes(value));
                            }
                            return bytes.ToArray();
                        }
                        else
                        {
                            throw new BytesSerializationException($"Type '{objType}' is not supported");
                        }
                    default: throw new BytesSerializationException($"Type '{objType}' is not supported");
                }
            }
            catch (Exception e)
            {
                throw new BytesSerializationException($"Unable to serialize {obj}", e);
            }
        }
        public object DeserializeFromBytes(byte[] bytes, Type targetType)
        {
            try
            {
                DeserializeFromBytesCore(bytes, targetType, out var result);
                return result;
            }
            catch (Exception e)
            {
                throw new BytesSerializationException($"Unable to deserialize bytes to {targetType}", e);
            }
        }
        public T DeserializeFromBytes<T>(byte[] bytes)
        {
            return DeserializeFromBytes<T>(bytes.AsSpan());
        }
        public T DeserializeFromBytes<T>(ReadOnlySpan<byte> bytes)
        {
            try
            {
                DeserializeFromBytesCore(bytes, typeof(T), out var result);
                return (T)result;
            }
            catch (Exception e)
            {
                throw new BytesSerializationException($"Unable to deserialize bytes to {typeof(T)}", e);
            }
        }

        public static void ClearFieldInfosCache()
        {
            _fieldInfosCache.Clear();
        }

        private static readonly BindingFlags _defaultBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        private static readonly Dictionary<Guid, FieldInfo[]> _fieldInfosCache = new();
        private static FieldInfo[] GetFieldInfos(Type type)
        {
            if (_fieldInfosCache.TryGetValue(type.GUID, out var result))
            {
                return result;
            }
            else
            {
                var fieldInfos = (from fieldInfo in type.GetFields(_defaultBindingFlags)
                                  let att = fieldInfo.GetCustomAttribute<BytesIncludedAttribute>()
                                  where att is not null
                                  orderby att.Order
                                  select fieldInfo).ToArray();
                _fieldInfosCache[type.GUID] = fieldInfos;
                return fieldInfos;
            }
        }
        private Int32 DeserializeFromBytesCore(ReadOnlySpan<byte> bytes, Type targetType, out object result)
        {
            switch (Type.GetTypeCode(targetType))
            {
                case TypeCode.Byte:
                    result = bytes[0];
                    return sizeof(Byte);
            }
            switch (Type.GetTypeCode(targetType))
            {
                case TypeCode.Byte: result = bytes[0]; return sizeof(Byte);
                case TypeCode.SByte: result = (SByte)bytes[0]; return sizeof(SByte);
                case TypeCode.Int16: result = BitConverter.ToInt16(bytes); return sizeof(Int16);
                case TypeCode.UInt16: result = BitConverter.ToUInt16(bytes); return sizeof(UInt16);
                case TypeCode.Int32: result = BitConverter.ToInt32(bytes); return sizeof(Int32);
                case TypeCode.UInt32: result = BitConverter.ToUInt32(bytes); return sizeof(UInt32);
                case TypeCode.Int64: result = BitConverter.ToInt64(bytes); return sizeof(Int64);
                case TypeCode.UInt64: result = BitConverter.ToUInt64(bytes); return sizeof(UInt64);
                case TypeCode.Single: result = BitConverter.ToSingle(bytes); return sizeof(Single);
                case TypeCode.Double: result = BitConverter.ToDouble(bytes); return sizeof(Double);
                case TypeCode.Decimal:
                    int[] decimalIntArray = new int[sizeof(Decimal) / sizeof(Int32)];
                    for (int i = 0; i < decimalIntArray.Length; i++)
                    {
                        decimalIntArray[i] = BitConverter.ToInt32(bytes.Slice(sizeof(Int32) * i, sizeof(Int32)));
                    }
                    result = new decimal(decimalIntArray);
                    return sizeof(Decimal);
                case TypeCode.Boolean: result = BitConverter.ToBoolean(bytes); return sizeof(Boolean);
                case TypeCode.Char: result = BitConverter.ToChar(bytes); return sizeof(Char);
                case TypeCode.DateTime: result = DateTime.FromBinary(BitConverter.ToInt64(bytes)); return sizeof(Int64);
                case TypeCode.String:
                    Int32 lengthInfo = BitConverter.ToInt32(bytes);
                    result = TextEncoding.GetString(bytes[sizeof(Int32)..(sizeof(Int32) + lengthInfo)]);
                    return sizeof(Int32) + lengthInfo;
                default: break;
            }

            if (targetType.IsArray)
            {
                Int32 arraySize = BitConverter.ToInt32(bytes);
                Int32 offset = 4;
                var elementType = targetType.GetElementType()!;
                var resultArray = Array.CreateInstance(elementType, arraySize);
                for (Int32 i = 0; i < arraySize; i++)
                {
                    offset += DeserializeFromBytesCore(bytes[offset..], elementType, out var value);
                    resultArray.SetValue(value, i);
                }
                result = resultArray;
                return offset;
            }
            else if (targetType.IsDefined(typeof(BytesSerializableAttribute)))
            {
                Int32 offset = 0;
                var obj = Activator.CreateInstance(targetType)!;
                foreach (var fieldInfo in GetFieldInfos(targetType))
                {
                    offset += DeserializeFromBytesCore(bytes[offset..], fieldInfo.FieldType, out var value);
                    fieldInfo.SetValue(obj, value);
                }
                result = obj;
                return offset;
            }
            else
            {
                throw new BytesSerializationException($"Type '{targetType}' is not supported");
            }
        }
    }
}
