using UnityEngine;

namespace State_Machine
{
    public abstract class Detector : MonoBehaviour
    {
        public abstract void Detect(Enemy enemy);
    }
}