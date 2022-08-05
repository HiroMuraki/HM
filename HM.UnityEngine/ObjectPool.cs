#nullable enable
using System.Collections.Generic;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HM.UnityEngine
{
    public class ObjectPool<T> where T : Component
    {
        /// <summary>
        /// 对象池的容量上限，若Capacity的大小小于Count，则会销毁多出的部分
        /// </summary>
        public int Capacity
        {
            get
            {
                return _capacity;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Capacity), value, string.Empty);
                }
                _capacity = value;
                while (_pooledObjects.Count > _capacity)
                {
                    Object.Destroy(_pooledObjects.Dequeue());
                }
            }
        }
        /// <summary>
        /// 当前对象池的大小
        /// </summary>
        public int Count => _pooledObjects.Count;
        /// <summary>
        /// 对象池生成对象时使用的预制体
        /// </summary>
        public T? Prefab { get; set; }
        /// <summary>
        /// 集合生成对象的父节点
        /// </summary>
        public Transform? ObjectsHandler { get; set; }

        /// <summary>
        /// 装满对象池至容量上限
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public void FullFillPool()
        {
            while (_pooledObjects.Count < _capacity)
            {
                if (Prefab is null)
                {
                    throw new ArgumentNullException(nameof(Prefab));
                }
                var obj = Object.Instantiate(Prefab);
                obj.transform.SetParent(ObjectsHandler);
                obj.gameObject.SetActive(false);
                _pooledObjects.Enqueue(obj);
            }
        }
        /// <summary>
        /// 清空对象池
        /// </summary>
        public void ClearPool()
        {
            while (_pooledObjects.TryDequeue(out var obj))
            {
                Object.Destroy(obj);
            }
        }
        /// <summary>
        /// 尝试从对象池获取对象，若对象池为空则返回false
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryFetch(out T? result)
        {
            /* 若实例队列不为空，则出列一个实例并将其启用
             *（若T实现了IPooledObject接口，则先调用其WhenFetch方法）*/
            if (_pooledObjects.TryDequeue(out var fetched))
            {
                result = FetchHelper(fetched);
                return true;
            }
            else
            {
                result = null;
                return true;
            }
        }
        /// <summary>
        /// 尝试将对象归还至对象池，若对象池已满则返回false
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool TryReturn(T gameObject)
        {
            /* 将目标实例禁用后，并将其父节点设为ObjectsHandler，再入列到实例队列中
             * （若T实现了IPooledObject接口，则先调用其WhenReturn方法）
             * 若队列大小已经达到容量上限，视为归还失败 */
            if (_pooledObjects.Count >= Capacity)
            {
                return false;
            }
            ReturnHelper(gameObject);
            return true;
        }
        /// <summary>
        /// 从对象池获取对象，若对象池为空则Instantiate一个Prefab
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T FetchOrInstantiate()
        {
            if (!TryFetch(out var result))
            {
                if (Prefab is null)
                {
                    throw new ArgumentNullException(nameof(Prefab));
                }
                result = Object.Instantiate(Prefab);
            }
            return result!;
        }
        /// <summary>
        /// 将归还至对象池，若对象池已满则将其销毁
        /// </summary>
        /// <param name="gameObject"></param>
        public void ReturnOrDestroy(T gameObject)
        {
            if (!TryReturn(gameObject))
            {
                Object.Destroy(gameObject);
            }
        }

        private int _capacity;
        private readonly Queue<T> _pooledObjects = new();
        private T ReturnHelper(T gameObject)
        {
            if (gameObject is IPooledObject pooledObject)
            {
                pooledObject.WhenReturn();
            }
            gameObject.gameObject.SetActive(false);
            gameObject.transform.SetParent(ObjectsHandler);
            _pooledObjects.Enqueue(gameObject);
            return gameObject;
        }
        private T FetchHelper(T gameObject)
        {
            if (gameObject is IPooledObject pooledObject)
            {
                pooledObject.WhenFetch();
            }
            gameObject.gameObject.SetActive(true);
            return gameObject;
        }
    }
}
