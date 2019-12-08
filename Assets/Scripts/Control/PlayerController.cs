using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private MobileController mController;

        private Health health;
        private CombatTarget target;

        void Start()
        {
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits;

            if (Input.GetMouseButton(0))
            {
                hits = Physics.RaycastAll(GetMouseRay());

                for (int i = hits.Length - 1; i >= 0; i--)
                {
                    target = hits[i].transform.GetComponent<CombatTarget>();

                    if (target == null || !GetComponent<Fighter>().CanAttack(target.gameObject))
                        continue;

                    GetComponent<Fighter>().Attack(target.gameObject);
                    return true;
                }
            }

            return false;
        }

        public bool InteractWithMovement()
        {
            Vector3 moveVector;
            moveVector = transform.position
                         + Vector3.right * mController.Horizontal()
                         + Vector3.forward * mController.Vertical();

            if (moveVector != transform.position)
            {
                GetComponent<Mover>().StartMoveAction(moveVector);
                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}
