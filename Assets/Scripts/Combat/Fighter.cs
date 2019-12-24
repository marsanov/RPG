using System;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Networking;

namespace RPG.Combat
{
    public class Fighter : NetworkBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        
        public Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private Weapon currentWeapon = null;
        private GameObject character;

        private Action OnHit;

        void Awake()
        {
            if (currentWeapon == null)
            {
                EquippWeapon(defaultWeapon);
            }
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;
            if (target.gameObject == this.gameObject) return;

            if (!GetIsInRange(target.transform))
            {
                GetComponent<Mover>().RpcMoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().RpcCancel();
                AttackBehaviour();
            }
        }

        public void EquippWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);

            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //this will trigger the Hit() event
                RpcTriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        [ClientRpc]
        private void RpcTriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeapon.GetRange();
        }

        [Command]
        public void CmdAttack(string targetId)
        {
            Health character = GameManager.GetPlayer(gameObject.name);

            character.GetComponent<ActionScheduler>().StartAction(this);
            character.GetComponent<Fighter>().target = GameManager.GetPlayer(targetId);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        [ClientRpc]
        public void RpcCancel()
        {
            RpcStopAttack();
            target = null;
            GetComponent<Mover>().RpcCancel();
        }
        
        [ClientRpc]
        private void RpcStopAttack()
        {
            GameManager.GetPlayer(gameObject.name).GetComponent<Animator>().ResetTrigger("attack");
            GameManager.GetPlayer(gameObject.name).GetComponent<Animator>().SetTrigger("stopAttack");
        }

        //Animation Event
        void Hit()
        {
            if (target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                CmdDealDamage(gameObject, damage, target.gameObject.name);
            }
        }

        [Command]
        void CmdDealDamage(GameObject gameObject, float damage, string targetID)
        {
            target = GameManager.GetPlayer(targetID);
            target.TakeDamage(gameObject, damage);
        }

        void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquippWeapon(weapon);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetDamage();
            }
        }

        public IEnumerable<float> GetPersantageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetPercentageBonus();
            }
        }
    }

}