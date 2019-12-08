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

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, startingLevel);
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