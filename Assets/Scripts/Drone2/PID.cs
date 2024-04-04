namespace Drone2
{
    [System.Serializable]
    public class Pid
    {
        public float pFactor;
        public float iFactor;
        public float dFactor;

        private float _integral;
        private float _lastError;

        public Pid(float pFactor, float iFactor, float dFactor)
        {
            this.pFactor = pFactor;
            this.iFactor = iFactor;
            this.dFactor = dFactor;
        }

        public float Update(float setpoint, float actual, float timeFrame)
        {
            var present = setpoint - actual;
            _integral += present * timeFrame;

            var deriv = (present - _lastError) / timeFrame;
            _lastError = present;

            var finalPid = present * pFactor + _integral * iFactor + deriv * dFactor;
            if ((finalPid > -0.1) && (finalPid < 0.1))
            {
                finalPid = 0;
            }

            return finalPid;
        }
    }
}