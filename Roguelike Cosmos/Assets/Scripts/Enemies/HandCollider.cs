using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour
{
    private lancer lancer;
    private void Awake()
    {
        lancer = GetComponentInParent<lancer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<HealthSystem>().TakeDamage(lancer.damage);
        }
    }
}
