#nullable enable
using System.Collections;

namespace HM.UnityEngine
{
    public class TimedListener : MonoBehaviour
    {
        public float Duration { get; set; }
        public bool IsListening { get; private set; }

        public void Active()
        {
            if (_lastTicker is not null)
            {
                StopCoroutine(_lastTicker);
            }
            _lastTicker = StartCoroutine(Ticker());
        }
        public void Deactive()
        {
            if (_lastTicker is not null)
            {
                StopCoroutine(_lastTicker);
            }
            IsListening = false;
        }

        private Coroutine? _lastTicker;
        IEnumerator Ticker()
        {
            IsListening = true;
            float timer = 0;
            while (true)
            {
                if (timer >= Duration)
                {
                    break;
                }
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            IsListening = false;
        }
    }
}