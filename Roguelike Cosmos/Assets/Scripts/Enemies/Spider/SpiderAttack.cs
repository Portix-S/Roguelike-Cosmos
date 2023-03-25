using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttack : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform launchPos;
    float projectileSpeed = 20f;
    int spiderDamage;
    public void LaunchPoison()
    {
        spiderDamage = GetComponentInParent<EnemyController>().GetDamage();
        var proj = Instantiate(projectile, launchPos.position, launchPos.rotation);
        proj.GetComponent<Rigidbody>().velocity = launchPos.forward * projectileSpeed;
        proj.GetComponent<SpiderProjectile>().SetDamage(spiderDamage);
    }
}
