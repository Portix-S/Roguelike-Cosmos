using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAttacks : MonoBehaviour
{
    // Start is called before the first frame update
    public MageBoss mb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AtkFireball()
    {
        mb.AtkFireball();
    }

    void Summon()
    {
        mb.SummonEnemy();
    }

    void AtkMeteor()
    {
        mb.AtkMeteor();
    }
}
