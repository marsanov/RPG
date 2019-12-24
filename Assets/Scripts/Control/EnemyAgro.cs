using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyAgro : MonoBehaviour
    {
        [SerializeField] private GameObject agroCharacter;

        void Start()
        {
            GetComponent<SphereCollider>().radius = agroCharacter.GetComponent<AIController>().chaseDistance;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                agroCharacter.GetComponent<AIController>().player = other.gameObject;
            }
        }
    }

}