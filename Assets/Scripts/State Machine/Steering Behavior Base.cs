using UnityEngine;

namespace State_Machine
{
    public abstract class SteeringBehaviorBase : MonoBehaviour
    {
        public abstract (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, Enemy enemy);
    }
}