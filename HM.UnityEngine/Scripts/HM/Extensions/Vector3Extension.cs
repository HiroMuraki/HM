using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace HM.UnityEngine.Scripts.HM.Extensions
{
    public static class Vector3Extension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 NewWithX(this Vector3 self, float x)
        {
            return new Vector3(x, self.y, self.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 NewWithY(this Vector3 self, float y)
        {
            return new Vector3(self.x, y, self.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 NewWithZ(this Vector3 self, float z)
        {
            return new Vector3(self.x, self.y, z);
        }
    }
}
