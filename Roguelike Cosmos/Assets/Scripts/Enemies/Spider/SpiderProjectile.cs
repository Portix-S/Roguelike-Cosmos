using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderProjectile : MonoBehaviour
{
    public float time = 3f;
    int damage;
    private void Awake()
    {
        Destroy(gameObject, time);
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit!");
            //Player Take Damage;
            Destroy(gameObject, 0.01f);

        }
    }
}
