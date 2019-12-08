using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

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
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }

        void Start()
        {
            maxHealthPoints = GetComponent<BaseStats>().GetHealth();
            HealthBarUpdate();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            HealthBarUpdate();
            if (healthPoints == 0)
            {
                Die();
            }
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
