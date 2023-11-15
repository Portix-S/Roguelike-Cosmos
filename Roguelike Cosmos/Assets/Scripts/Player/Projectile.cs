using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    public float time = 0.5f;
    [SerializeField] float damage;
    [SerializeField] ParticleSystem trail1;
    [SerializeField] ParticleSystem trail2;

    private void Awake() {
        Invoke("DestroyFireball", 0.35f);
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
            Destroy(gameObject,0.01f);
        }
    }

    void DestroyFireball(){
        trail1.transform.parent = null;
        trail2.transform.parent = null;

        Destroy(trail1.gameObject, 2f);
        Destroy(trail2.gameObject, 2f);

        Destroy(this.gameObject);
    }
}
