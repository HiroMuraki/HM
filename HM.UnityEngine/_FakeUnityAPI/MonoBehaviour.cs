using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine
{
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
}
