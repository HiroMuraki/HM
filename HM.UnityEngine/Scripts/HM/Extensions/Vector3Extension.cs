using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace HM.UnityEngine
{
    public static class Vector3Extension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithX(this Vector3 self, float x)
        {
            return new Vector3(x, self.y, self.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithY(this Vector3 self, float y)
        {
            return new Vector3(self.x, y, self.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithZ(this Vector3 self, float z)
        {
            return new Vector3(self.x, self.y, z);
        }
    }
}
