using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    [System.Serializable]
    public struct FiniteTimer
    {
        public float Timer;
        public float MaxValue;

        public void init(float maxval, float startVal = 0)
        {
            Timer = startVal;
            MaxValue = maxval;
        }

        public FiniteTimer(float timerStart, float maxVal, bool completed = false)
        {
            Timer = completed ? maxVal : timerStart;
            MaxValue = maxVal;
        }

        public FiniteTimer(float maxVal = 3, bool completed = false)
        {
            Timer = completed ? maxVal : 0;
            MaxValue = maxVal;
        }




        public void updateTimer(float delta)
        {
            Timer += delta;
        }
        /// <summary>
        /// return true if this is completed before or after updating
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public bool tryCompleteTimer(float delta)
        {
            return tryCompleteTimer(delta, out var r);
        }
        
        /// <summary>
        /// return true if this is completed after updating only if it wasn't completed before
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public bool tryCompleteOnce(float delta)
        {
            if (isComplete)
            {
                return false;
            }
            return tryCompleteTimer(delta, out var r);
        }

        public bool tryCompleteTimer(float delta, out float remainder)
        {
            remainder = delta;
            if (isComplete)
                return true;
            Timer += delta;
            if (Timer > MaxValue)
            {
                remainder = Timer - MaxValue;
                return true;
            }

            remainder = 0;
            return false;
        }
        
        public bool tryEmptyTimer(float delta)
        {
            if (Timer <= 0)
                return true;

            if (Timer <= delta)
            {
                Timer = 0;
                return true;
            }
            else
            {
                Timer -= delta;
            }
            return false;
        }

        public bool tryEmptyTimer(float delta, out float remainder)
        {
            remainder = delta;
            if (Timer <= 0)
                return true;

            if (Timer <= delta)
            {
                Timer = 0;
                remainder -= Timer;
                return true;
            }
            else
            {
                Timer -= remainder;
                remainder = 0;
            }
            return false;
        }

        public void clampedUpdateTimer(float delta)
        {
            Timer = Mathf.Clamp(Timer + delta, 0, MaxValue);
        }

        public void safeUpdateTimer(float delta)
        {
            Timer = Mathf.Clamp(Timer + delta, 0, MaxValue);
        }
        
        public void safeSubtractTimer(float delta)
        {
            Timer = Mathf.Clamp(Timer - delta, 0, MaxValue);
        }
        
        public void subtractTimer(float delta)
        {
            Timer -= delta;
        }


        public void reset()
        {
            Timer = 0;
        }

        public void resetByFractionOfMax(float frac)
        {
            Timer = Mathf.Max(0, Timer - frac * MaxValue);
        }
        
        /// <summary>
        /// Reset and set max
        /// </summary>
        /// <param name="newMax"></param>
        public void reset(float newMax)
        {
            MaxValue = newMax;
            reset();
        }
        
        public void resetAndSetToMax(float newMax)
        {
            Timer = MaxValue = newMax;
        }

        public void resetAndSetMax(float newMax)
        {
            MaxValue = newMax;
            reset();
        }
        public void resetAndKeepExtra()
        {
            if (Timer > MaxValue)
                Timer -= MaxValue;
            else
                reset();
        }

        public void complete()
        {
            Timer = MaxValue;
        }

        public bool isComplete => Timer >= MaxValue;
        public bool isEmpty => Timer <=0;

        public bool exceedsRatio(float ratioToExceed)
        {
            return Ratio >= ratioToExceed;
        }

        public float Ratio => Mathf.Clamp01(Timer / MaxValue);

        public float ReverseRatio => 1 - Ratio;

        public float remaingValue()
        {
            if (Timer >= MaxValue)
                return 0;
            return MaxValue - Timer;
        }

        public override string ToString()
        {
            return $"{Timer}/{MaxValue} {Ratio*100}%";
        }
    }
    
    [System.Serializable]
    public class SafeFiniteTimer
    {
        public float Timer;
        public float MaxValue;

        public void init(float maxval, float startVal = 0)
        {
            Timer = startVal;
            MaxValue = maxval;
        }

        public SafeFiniteTimer(float timerStart, float maxVal, bool completed = false)
        {
            Timer = completed ? maxVal : timerStart;
            MaxValue = maxVal;
        }

        public SafeFiniteTimer(float maxVal = 3, bool completed = false)
        {
            Timer = completed ? maxVal : 0;
            MaxValue = maxVal;
        }




        public void updateTimer(float delta)
        {
            Timer += delta;
        }
        /// <summary>
        /// return true if this is completed before or after updating
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public bool tryCompleteTimer(float delta)
        {
            return tryCompleteTimer(delta, out var r);
        }

        public bool tryCompleteTimer(float delta, out float remainder)
        {
            remainder = delta;
            if (isComplete)
                return true;
            Timer += delta;
            if (Timer > MaxValue)
            {
                remainder = Timer - MaxValue;
                return true;
            }

            remainder = 0;
            return false;
        }
        
        
        public bool tryEmptyTimer(float delta, out float remainder)
        {
            remainder = delta;
            if (Timer <= 0)
                return true;

            if (Timer <= delta)
            {
                Timer = 0;
                remainder -= Timer;
                return true;
            }
            else
            {
                Timer -= remainder;
                remainder = 0;
            }
            return false;
        }

        public void clampedUpdateTimer(float delta)
        {
            Timer = Mathf.Clamp(Timer + delta, 0, MaxValue);
        }

        public void safeUpdateTimer(float delta)
        {
            if (Timer < MaxValue)
                Timer += delta;
        }

        public void reset()
        {
            Timer = 0;
        }

        public void resetByFractionOfMax(float frac)
        {
            Timer = Mathf.Max(0, Timer - frac * MaxValue);
        }
        
        /// <summary>
        /// Reset and set max
        /// </summary>
        /// <param name="newMax"></param>
        public void reset(float newMax)
        {
            MaxValue = newMax;
            reset();
        }

        public void resetAndKeepExtra()
        {
            if (Timer > MaxValue)
                Timer -= MaxValue;
            else
                reset();
        }

        public void complete()
        {
            Timer = MaxValue;
        }

        public bool isComplete => Timer >= MaxValue;

        public bool exceedsRatio(float ratioToExceed)
        {
            return Ratio >= ratioToExceed;
        }

        public float Ratio => Mathf.Clamp01(Timer / MaxValue);

        public float ReverseRatio => 1 - Ratio;
    }
}