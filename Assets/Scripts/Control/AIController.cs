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
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspitionTime = 3f;
        [SerializeField] private float waypointDwellTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;

        private GameObject Player;
        private Fighter fighter;
        private Health health;
        private Mover mover;
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinseArrivedAtWaypoint = Mathf.Infinity;
        private int currentWaypointIndex = 0;

        void Start()
        {
            Player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardPosition = transform.position;
        }

        void Update()
        {
            if (health.IsDead()) return;
            if(Player == null) Player = GameObject.FindWithTag("Player");

            if (InAttackRangeOfPlayer() && fighter.CanAttack(Player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspitionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
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
                mover.StartMoveAction(nextPosition);
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

        
        #region Attack
        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            GetComponent<Fighter>().Attack(Player.name);
            GetComponent<NavMeshAgent>().speed = 4.5f;
        }

        private bool InAttackRangeOfPlayer()
        {
            if (Player == null) return false;
            float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
            return distanceToPlayer < chaseDistance;
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