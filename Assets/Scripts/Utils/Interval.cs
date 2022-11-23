using UnityEngine;

namespace Utils
{
    public class Interval
    {
        private float seconds;
        private float timeStamp;

        public Interval(float seconds)
        {
            Set(seconds);
        }

        public void Set(float seconds)
        {
            this.seconds = seconds;
            timeStamp = Time.time;
        }

        public bool ExpireReset()
        {
            if (Time.time >= timeStamp + seconds)
            {
                Set(seconds);
                return true;
            }

            return false;
        }
    }
}