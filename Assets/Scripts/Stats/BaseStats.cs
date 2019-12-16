using System;
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
        [SerializeField] private GameObject levelUpParticleEffect = null;
        [SerializeField] private bool shouldUseModifiers = false;

        private int currentLevel = 0;

        public event Action onLevelUp;

        void Start()
        {
            currentLevel = CalculateLevel();
            Expirience expirience = GetComponent<Expirience>();
            if (expirience != null)
            {
                expirience.onExpirienceGained += UpdateLevel;
            }
        }

        void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifiers(stat) / 100);
        }
        
        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in modifierProvider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private float GetPercentageModifiers(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in modifierProvider.GetPersantageModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        public object CaptureState()
        {
            return startingLevel;
        }


        public void RestoreState(object state)
        {
            startingLevel = (int) state;
        }

        public int CalculateLevel()
        {
            Expirience expirience = GetComponent<Expirience>();
            if (expirience == null) return startingLevel;

            float currentXP = expirience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExpirienceToLevelUp, characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
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