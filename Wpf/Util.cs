using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HM.Wpf
{
    public static class Util
    {
        public static async Task<int> GetMouseButtonHeldTime(MouseDevice mouseDevice, int timeOut)
        {
            int delayMS = 10;
            int heltTime = 0;
            while (true)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(delayMS));
                heltTime += delayMS;
                if (mouseDevice.LeftButton != MouseButtonState.Pressed || (timeOut >= 0 && heltTime >= timeOut))
                {
                    return heltTime;
                }
            }
        }
    }
}