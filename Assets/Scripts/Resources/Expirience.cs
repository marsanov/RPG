using RPG.Saving;
using UnityEngine;

namespace RPG.Resources
{
    public class Expirience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float expiriencePints = 0;
        
        public void GainExpirience(float expirience)
        {
            expiriencePints += expirience;
        }

        public object CaptureState()
        {
            return expiriencePints;
        }

        public void RestoreState(object state)
        {
            expiriencePints = (float) state;
        }
    }
}