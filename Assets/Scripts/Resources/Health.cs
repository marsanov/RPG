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
using UnityEditor;
using UnityEngine.Networking;

namespace RPG.Resources
{
    public class Health : NetworkBehaviour, ISaveable
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] private Image healthBar;
        
        [SyncVar]
        float healthPoints = -1f;
        [SyncVar]
        private bool isDead = false;
        
        void Start()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        private void RegenerateHealth()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        void Update()
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
            Debug.Log(gameObject.name + " took damage: " + damage);

            healthPoints = Mathf.Max(healthPoints - damage, 0);
            HealthBarUpdate();
            if (healthPoints == 0)
            {
                RpcDie();
                AwardExperience(instigator);
            }
        }
        
        public float GetHealthPoints()
        {
            return healthPoints;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(
                Stat.Health);
        }

        private void AwardExperience(GameObject instigator)
        {
            Expirience expirience = instigator.GetComponent<Expirience>();
            if(expirience == null) return;

            float expirienceReward = GetComponent<BaseStats>().GetStat(Stat.ExpirienceRaward);
            expirience.RpcGainExpirience(expirienceReward);
        }
        
        [ClientRpc]
        private void RpcDie()
        {
            if(isDead) return;
            
            isDead = true;

            GetComponent<Animator>().SetTrigger("die");
            GetComponent<CapsuleCollider>().enabled = false;
            transform.GetChild(1).gameObject.SetActive(false);
            GetComponent<ActionScheduler>().CancelCurrentAction();

            if (gameObject.tag == "Enemy")
                transform.GetChild(2).GetComponent<CapsuleCollider>().enabled = false;
        }

        

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            HealthBarUpdate();
            if (healthPoints <= 0)
            {
                RpcDie();
            }
        }

        private void HealthBarUpdate()
        {
            healthBar.fillAmount = healthPoints / maxHealthPoints;
        }
    }
}
