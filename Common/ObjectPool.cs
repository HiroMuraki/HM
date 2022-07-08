#pragma warning disable IDE0049 // 强制使用.Net CL类型名


namespace HM.Common
{
    public class ObjectPool<T> where T : class
    {
        public int MaxSize
        {
            get => _maxSize;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"Value should not be smaller than zero");
                }
                _maxSize = value;
                while (Size >= MaxSize)
                {
                    _objectPool.Dequeue();
                }
            }
        }
        public int Size => _objectPool.Count;

        public T? FetchOrDefault(T? defaultValue)
        {
            if (TryFetchObject(out var result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }
        public void FillUp(Func<T>? objectCreater)
        {
            ArgumentNullException.ThrowIfNull(objectCreater);

            while (_objectPool.Count < MaxSize)
            {
                _objectPool.Enqueue(objectCreater());
            }
        }
        public bool TryFetchObject(out T? result)
        {
            /* 尝试从数据队列中获取，若无法获取但允许new一个 */
            if (_objectPool.TryDequeue(out result))
            {
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
        public bool TryReturnObject(T obj)
        {
            /* 容量达到上限后则直接忽略，否则将其加入到队尾,
             * 若实现了IResetable接口，则调用相应的接口方法 */
            if (Size >= MaxSize) return false;

            if (obj is IResetable resetable)
            {
                resetable.Reset();
            }
            _objectPool.Enqueue(obj);
            return true;
        }

        public ObjectPool(int maxSize)
        {
            MaxSize = maxSize;
        }
        public ObjectPool()
        {

        }

        private readonly Queue<T> _objectPool = new();
        private int _maxSize;
    }
}
