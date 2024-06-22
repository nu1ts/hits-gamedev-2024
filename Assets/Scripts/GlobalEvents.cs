using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GlobalEvents
{
    public static event Action<float, float> OnExplosion;

    public static void TriggerCameraShake(float duration, float magnitude)
    {
        OnExplosion?.Invoke(duration, magnitude);
    }
}
