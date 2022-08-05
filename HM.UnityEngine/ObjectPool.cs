#nullable enable
using System.Collections.Generic;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HM.UnityEngine
{
    public class ObjectPool
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
        public GameObject? Prefab { get; set; }
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
                obj.SetActive(false);
                _pooledObjects.Enqueue(obj);
            }
        }
        /// <summary>
        /// 清空对象池
        /// </summary>
        public void ClearPool()
        {
            while (_pooledObjects.TryDequeue(out var gameObject))
            {
                Object.Destroy(gameObject);
            }
        }
        /// <summary>
        /// 尝试从对象池获取对象，若对象池为空则返回false
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryFetch(out GameObject? result)
        {
            if (_pooledObjects.TryDequeue(out var fetched))
            {
                result = FetchHelper(fetched);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
        /// <summary>
        /// 尝试将对象归还至对象池，若对象池已满则返回false
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool TryReturn(GameObject gameObject)
        {
            if (_pooledObjects.Count >= Capacity)
            {
                return false;
            }
            ReturnHelper(gameObject);
            _pooledObjects.Enqueue(gameObject);
            return true;
        }
        /// <summary>
        /// 从对象池获取对象，若对象池为空则Instantiate一个Prefab
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public GameObject FetchOrInstantiate()
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
        public void ReturnOrDestroy(GameObject gameObject)
        {
            if (!TryReturn(gameObject))
            {
                Object.Destroy(gameObject);
            }
        }

        private int _capacity;
        private readonly Queue<GameObject> _pooledObjects = new();
        private GameObject ReturnHelper(GameObject gameObject)
        {
            /* 调用其所有实现IPooledObject接口的Component的WhenFetch方法，然后将其禁用 */
            foreach (var pooledObject in gameObject.GetComponents<IPooledObject>())
            {
                pooledObject.WhenReturn();
            }
            gameObject.SetActive(false);
            gameObject.transform.SetParent(ObjectsHandler);
            return gameObject;
        }
        private GameObject FetchHelper(GameObject gameObject)
        {
            /* 调用其所有实现IPooledObject接口的Component的WhenFetch方法，然后将其启用 */
            foreach (var pooledObject in gameObject.GetComponents<IPooledObject>())
            {
                pooledObject.WhenFetch();
            }
            gameObject.SetActive(true);
            return gameObject;
        }
    }
}
