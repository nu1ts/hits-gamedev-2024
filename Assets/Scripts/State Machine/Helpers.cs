using UnityEngine;

namespace State_Machine
{
    public static class Helpers
    {
        public static float Map(float value, float sourceMin, float sourceMax, float targetMin, float targetMax, bool clamp = false) {
            var normalizedValue = (value - sourceMin) / (sourceMax - sourceMin);
            var mappedValue = targetMin + (targetMax - targetMin) * normalizedValue;
            
            if (clamp) {
                mappedValue = Mathf.Clamp(mappedValue, Mathf.Min(targetMin, targetMax), Mathf.Max(targetMin, targetMax));
            }

            return mappedValue;
        }

    }
}