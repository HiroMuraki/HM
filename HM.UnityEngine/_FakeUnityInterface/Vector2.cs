using System;

namespace UnityEngine
{
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
