#pragma warning disable IDE0049
using System.IO;
using System.Reflection;
using System.Text;

namespace HM.Serialization.BytesSerialization
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
                    case TypeCode.SByte: return new byte[] { (byte)(sbyte)obj };
                    case TypeCode.Byte: return new byte[] { (byte)obj };
                    case TypeCode.Int16: return BitConverter.GetBytes((short)obj!);
                    case TypeCode.UInt16: return BitConverter.GetBytes((ushort)obj!);
                    case TypeCode.Int32: return BitConverter.GetBytes((int)obj!);
                    case TypeCode.UInt32: return BitConverter.GetBytes((uint)obj!);
                    case TypeCode.Int64: return BitConverter.GetBytes((long)obj!);
                    case TypeCode.UInt64: return BitConverter.GetBytes((ulong)obj!);
                    case TypeCode.Single: return BitConverter.GetBytes((float)obj!);
                    case TypeCode.Double: return BitConverter.GetBytes((double)obj!);
                    case TypeCode.Decimal:
                        int[] intArray = decimal.GetBits((decimal)obj!);
                        byte[] decimalBytes = new byte[sizeof(decimal)];
                        for (int i = 0; i < intArray.Length; i++)
                        {
                            Array.Copy(BitConverter.GetBytes(intArray[i]), 0, decimalBytes, sizeof(int) * i, sizeof(int));
                        }
                        return decimalBytes;
                    case TypeCode.Boolean: return BitConverter.GetBytes((bool)obj!);
                    case TypeCode.Char: return BitConverter.GetBytes((char)obj!);
                    case TypeCode.DateTime: return BitConverter.GetBytes(((DateTime)obj!).Ticks);
                    case TypeCode.String:
                        // 编码字符串，写入编码后的字节数组长度 + 字节数组长度
                        byte[] encoded = TextEncoding.GetBytes((string)obj!);
                        byte[] strLengthInfo = BitConverter.GetBytes(encoded.Length);
                        byte[] result = new byte[sizeof(int) + encoded.Length];
                        Array.Copy(strLengthInfo, 0, result, 0, strLengthInfo.Length);
                        Array.Copy(encoded, 0, result, sizeof(int), encoded.Length);
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
                            for (int i = 0; i < array.Length; i++)
                            {
                                var element = array.GetValue(i) ?? throw new NullReferenceException("Element in array can't be null");
                                objBytes.AddRange(SerializeToBytes(element));
                            }

                            byte[] arrayResult = new byte[sizeof(int) + objBytes.Count];
                            Array.Copy(lengthInfo, 0, arrayResult, 0, lengthInfo.Length);
                            Array.Copy(objBytes.ToArray(), 0, arrayResult, sizeof(int), objBytes.Count);
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
        public object DeserializeFromBytes(Stream stream, Type targetType)
        {
            return DeserializeFromBytes(GetDataFromSteam(stream), targetType);
        }
        public object DeserializeFromBytes(byte[] bytes, Type targetType)
        {
            return DeserializeFromBytes(bytes.AsSpan(), targetType);
        }
        public object DeserializeFromBytes(ReadOnlySpan<byte> bytes, Type targetType)
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
        public T DeserializeFromBytes<T>(Stream stream)
        {
            return DeserializeFromBytes<T>(GetDataFromSteam(stream));
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
        private int DeserializeFromBytesCore(ReadOnlySpan<byte> bytes, Type targetType, out object result)
        {
            switch (Type.GetTypeCode(targetType))
            {
                case TypeCode.Byte:
                    result = bytes[0];
                    return sizeof(byte);
            }
            switch (Type.GetTypeCode(targetType))
            {
                case TypeCode.Byte: result = bytes[0]; return sizeof(byte);
                case TypeCode.SByte: result = (sbyte)bytes[0]; return sizeof(sbyte);
                case TypeCode.Int16: result = BitConverter.ToInt16(bytes); return sizeof(short);
                case TypeCode.UInt16: result = BitConverter.ToUInt16(bytes); return sizeof(ushort);
                case TypeCode.Int32: result = BitConverter.ToInt32(bytes); return sizeof(int);
                case TypeCode.UInt32: result = BitConverter.ToUInt32(bytes); return sizeof(uint);
                case TypeCode.Int64: result = BitConverter.ToInt64(bytes); return sizeof(long);
                case TypeCode.UInt64: result = BitConverter.ToUInt64(bytes); return sizeof(ulong);
                case TypeCode.Single: result = BitConverter.ToSingle(bytes); return sizeof(float);
                case TypeCode.Double: result = BitConverter.ToDouble(bytes); return sizeof(double);
                case TypeCode.Decimal:
                    int[] decimalIntArray = new int[sizeof(decimal) / sizeof(int)];
                    for (int i = 0; i < decimalIntArray.Length; i++)
                    {
                        decimalIntArray[i] = BitConverter.ToInt32(bytes.Slice(sizeof(int) * i, sizeof(int)));
                    }
                    result = new decimal(decimalIntArray);
                    return sizeof(decimal);
                case TypeCode.Boolean: result = BitConverter.ToBoolean(bytes); return sizeof(bool);
                case TypeCode.Char: result = BitConverter.ToChar(bytes); return sizeof(char);
                case TypeCode.DateTime: result = DateTime.FromBinary(BitConverter.ToInt64(bytes)); return sizeof(long);
                case TypeCode.String:
                    int lengthInfo = BitConverter.ToInt32(bytes);
                    result = TextEncoding.GetString(bytes[sizeof(int)..(sizeof(int) + lengthInfo)]);
                    return sizeof(int) + lengthInfo;
                default: break;
            }

            if (targetType.IsArray)
            {
                int arraySize = BitConverter.ToInt32(bytes);
                int offset = 4;
                var elementType = targetType.GetElementType()!;
                var resultArray = Array.CreateInstance(elementType, arraySize);
                for (int i = 0; i < arraySize; i++)
                {
                    offset += DeserializeFromBytesCore(bytes[offset..], elementType, out var value);
                    resultArray.SetValue(value, i);
                }
                result = resultArray;
                return offset;
            }
            else if (targetType.IsDefined(typeof(BytesSerializableAttribute)))
            {
                int offset = 0;
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
        private Span<byte> GetDataFromSteam(Stream stream)
        {
            byte[] data = new byte[stream.Length];
            byte[] buffer = new byte[8192];
            int readCount = 0;
            int nextPos = 0;
            while ((readCount = stream.Read(buffer)) != 0)
            {
                for (int i = 0; i < readCount; i++)
                {
                    data[nextPos + i] = buffer[i];
                }
                nextPos += readCount;
            }
            return data.AsSpan();
        }
    }
}
