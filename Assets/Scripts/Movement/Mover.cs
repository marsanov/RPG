using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Resources;
using UnityEngine.Networking;

namespace RPG.Movement
{
    public class Mover : NetworkBehaviour, IAction, ISaveable
    {
        private Health health;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        [SyncVar]
        private Vector3 localVelocity;
        private GameObject character;

        void Awake()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Start()
        {
            character = GameManager.GetPlayer(gameObject.name).gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            RpcUpdateAnimator();
        }

        [Command]
        public void CmdStartMoveAction(Vector3 destination)
        {
            character.GetComponent<ActionScheduler>().StartAction(this);
            character.GetComponent<Mover>().RpcMoveTo(destination);
        }
        
        [ClientRpc]
        public void RpcCancel()
        {
            if (navMeshAgent.enabled == false) return;
            navMeshAgent.isStopped = true;
        }

        [ClientRpc]
        void RpcUpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            character.GetComponent<Mover>().localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            character.GetComponent<Mover>().animator.SetFloat("ForwardSpeed", speed);
        }

        [ClientRpc]
        public void RpcMoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.position);
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;
            navMeshAgent.enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}