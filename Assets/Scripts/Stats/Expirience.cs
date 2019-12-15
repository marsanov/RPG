using RPG.Saving;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class Expirience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float expiriencePints = 0;
        
        public event Action onExpirienceGained;

        public void GainExpirience(float expirience)
        {
            expiriencePints += expirience;
            onExpirienceGained();
        }

        public object CaptureState()
        {
            return expiriencePints;
        }

        public void RestoreState(object state)
        {
            expiriencePints = (float) state;
        }

        public float GetPoints()
        {
            return expiriencePints;
        }
    }
}