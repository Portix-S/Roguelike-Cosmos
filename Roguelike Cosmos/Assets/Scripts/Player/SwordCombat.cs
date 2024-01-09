using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCombat : MonoBehaviour
{
    PlayerCombat playerCombat;
    // Start is called before the first frame update
    void Start()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit :" + other.name);
        playerCombat.Attack(other);
    }
    
}
