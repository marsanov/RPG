using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using Rpg.Stats;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] private Image healthBar;

        private bool isDead = false;

        void Awake()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        void Start()
        {
            maxHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            HealthBarUpdate();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            HealthBarUpdate();
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Expirience expirience = instigator.GetComponent<Expirience>();
            if(expirience == null) return;

            float expirienceReward = GetComponent<BaseStats>().GetStat(Stat.ExpirienceRaward);
            expirience.GainExpirience(expirienceReward);
        }

        private void Die()
        {
            if(isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<CapsuleCollider>().enabled = false;
            transform.GetChild(1).gameObject.SetActive(false);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            HealthBarUpdate();
            if (healthPoints == 0)
            {
                Die();
            }
        }

        private void HealthBarUpdate()
        {
            healthBar.fillAmount = healthPoints / maxHealthPoints;
        }
    }
}
