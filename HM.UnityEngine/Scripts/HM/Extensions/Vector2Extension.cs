using System.Runtime.CompilerServices;
using HM.UnityEngine._FakeUnityAPI;

namespace HM.UnityEngine.Scripts.HM.Extensions
{
    public static class Vector2Extension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 NewWithX(this Vector2 self, float x)
        {
            return new Vector2(x, self.y);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 NewWithY(this Vector2 self, float y)
        {
            return new Vector2(self.x, y);
        }
    }
}
