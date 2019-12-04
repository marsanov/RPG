using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] private Image healthBar;

        private bool isDead = false;

        void Start()
        {
            CheckMaxHealthPoints();
        }

        private void CheckMaxHealthPoints()
        {
            if (maxHealthPoints < healthPoints)
                maxHealthPoints = healthPoints;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            healthBar.fillAmount = healthPoints / maxHealthPoints;
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
            GetComponent<NavMeshAgent>().enabled = false;
            transform.GetChild(1).gameObject.SetActive(false);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float) state;
            CheckMaxHealthPoints();
            healthBar.fillAmount = healthPoints / maxHealthPoints;
            if (healthPoints == 0)
            {
                Die();
            }
        }
    }
}
