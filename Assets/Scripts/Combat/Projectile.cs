using UnityEngine;
using  RPG.Resources;
using UnityEngine.Networking;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private bool isHoming = true;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private GameObject[] destroyOnHit = null;
        [SerializeField] private float lifeAfterImpact = 2f;

        private Health target = null;
        private GameObject instigator = null;
        private float damage = 0;

        void Start()
        {
            transform.LookAt(GetAimLocation());

            Destroy(gameObject, maxLifeTime);
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null) return;

            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsuleCollider = target.GetComponent<CapsuleCollider>();
            if (targetCapsuleCollider == null)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * targetCapsuleCollider.height / 2;
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            if (other.name != "AttackCollider") return;
            if (other.transform.parent.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;

            target.TakeDamage(instigator, damage);

            speed = 0;

            if (hitEffect != null)
            {
                GameObject _hitEffect = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                NetworkServer.Spawn(_hitEffect);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
            NetworkServer.Destroy(gameObject);
        }
    }

}