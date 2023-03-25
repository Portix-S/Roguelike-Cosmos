using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    public float time = 10f;
    [SerializeField] float damage;

    private void Awake() {
        Destroy(gameObject, time);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit!");
            other.GetComponent<EnemyController>().TakeDamage(damage);
        }
        Destroy(gameObject,0.01f);
    }
}
