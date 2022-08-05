using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine
{
    public class Time
    {
        public static float deltaTime = -1;
    }
    public class WaitForEndOfFrame
    {
    }
    public class Coroutine
    {
    }
    public class MonoBehaviour
    {
        protected Coroutine StartCoroutine(IEnumerator coroutine)
        {
            throw new InvalidOperationException();
        }
        protected Coroutine StopCoroutine(Coroutine coroutine)
        {
            throw new InvalidOperationException();
        }
    }
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            throw new InvalidOperationException();
        }
        public static Vector2 operator /(Vector2 vector, float val)
        {
            throw new InvalidOperationException();
        }
        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            throw new InvalidOperationException();
        }
        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            throw new InvalidOperationException();
        }
    }
}
