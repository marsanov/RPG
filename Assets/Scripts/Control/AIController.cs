using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace RPG.Control
{
    public class AIController : NetworkBehaviour
    {
        public float chaseDistance = 5f;
        public GameObject player;

        [SerializeField] private float suspitionTime = 3f;
        [SerializeField] private float waypointDwellTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;

        private Fighter fighter;
        private Health health;
        private Mover mover;
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinseArrivedAtWaypoint = Mathf.Infinity;
        private int currentWaypointIndex = 0;
        private bool isFighting = false;

        void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardPosition = transform.position;
            GetComponent<SphereCollider>().radius = chaseDistance;
        }

        void Update()
        {
            if (health.IsDead()) return;
            if(player == null) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                isFighting = true;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspitionTime)
            {
                isFighting = false;
                SuspicionBehaviour();
            }
            else
            {
                isFighting = false;
                PatrolBehaviour();
            }

            UpdateTimes();
        }

        
        #region Patrol
        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinseArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinseArrivedAtWaypoint > waypointDwellTime)
            {
                mover.CmdStartMoveAction(nextPosition);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }
        #endregion

        
        #region CmdAttack
        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            GetComponent<Fighter>().CmdAttack(player.name);
            GetComponent<NavMeshAgent>().speed = 4.5f;
        }

        private bool InAttackRangeOfPlayer()
        {
            if (player == null) return false;
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !isFighting)
                player = other.gameObject;
        }
        #endregion

        private void UpdateTimes()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinseArrivedAtWaypoint += Time.deltaTime;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().speed = 2f;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }

}