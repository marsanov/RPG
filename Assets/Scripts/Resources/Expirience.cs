using UnityEngine;

namespace RPG.Resources
{
    public class Expirience : MonoBehaviour
    {
        [SerializeField] private float expiriencePints = 0;

        public void GainExpirience(float expirience)
        {
            expiriencePints += expirience;
        }
    }
}