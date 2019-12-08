using Rpg.Stats;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour, ISaveable
    {
        [Range(1f, 99f)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null;

        public float GetHealth()
        {
            return progression.GetHealth(characterClass, startingLevel);
        }


        public object CaptureState()
        {
            return startingLevel;
        }


        public void RestoreState(object state)
        {
            startingLevel = (int) state;
        }
    }

}