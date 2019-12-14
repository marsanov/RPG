using Rpg.Stats;
using RPG.Resources;
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

        void Update()
        {
            if (gameObject.tag == "Player")
                print(GetLevel());
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }


        public object CaptureState()
        {
            return startingLevel;
        }


        public void RestoreState(object state)
        {
            startingLevel = (int) state;
        }

        public int GetLevel()
        {
            Expirience expirience = GetComponent<Expirience>();
            if (expirience == null) return startingLevel;

            float currentXP = expirience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExpirienceToLevelUp, characterClass);

            for (int level = 1; level < penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExpirienceToLevelUp, characterClass, level);

                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }

}