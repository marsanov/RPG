using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private bool isHoming = true;

    private Health target = null;
    private float damage = 0;

    void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        if (isHoming || !target.IsDead())
        {
            transform.LookAt(GetAimLocation());
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
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
        if (other.GetComponent<Health>() != target) return;
        if(target.IsDead()) return;
        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}
