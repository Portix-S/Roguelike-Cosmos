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
            other.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(gameObject,0.01f);
        }
        else if (other.gameObject.tag == "Boss")
        {
            other.GetComponent<MageBoss>().TakeDamage(damage);
            Destroy(gameObject, 0.01f);
        }
        else if (other.gameObject.tag == "Lancer")
        {
            other.GetComponent<lancer>().TakeDamage(damage);
            Destroy(gameObject, 0.01f);
        }
        else if (other.gameObject.tag == "Tentacle")
        {
            other.GetComponent<TentacleController>().TakeDamage(damage);
            Destroy(gameObject, 0.01f);
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
