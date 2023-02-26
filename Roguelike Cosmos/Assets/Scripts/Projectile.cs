using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    public float time = 10f;

    private void Awake() {
        Destroy(gameObject, time);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemie")
            Debug.Log("Hit!");
        Destroy(gameObject);
    }
}
