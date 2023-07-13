using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class HealthSystem : MonoBehaviour
{
    public PlayerData info; // Informações do player
    private float invbtyTime = 0.5f; // Tempo de invencibilidade após receber dano
    private float timeStamp; // Registra o tempo que o player vai poder levar dano novamente
    private int maxHeath; // Vida máxima
    void Start()
    {
        timeStamp = 0;
        maxHeath = 100;
    }


    void Cooldown()
    {

    }

    private void Update() {
        if(Input.GetKeyDown("b")) TakeDamage(10);
        if(Input.GetKeyDown("v")) Heal(20);
            
    }

    public void Heal(int h) 
    {
        /*
            Para curar o player
        */
        if(info.healthPoints+h > maxHeath)
            h = maxHeath - info.healthPoints;
        

        info.healthPoints += h;
    }

    void SetMaxHealth(int h)
    {
        if(h>0) maxHeath = h;
    }

    public void TakeDamage(int d) 
    {
        /*
            Para tomar dano e atribui o tempo que o player 
            vai poder receber dano de novo.
            Pode ser chamada pelo objeto que vai dar dano
            na hora do contato.
        */
        if(timeStamp > Time.time) return;

        if(info.healthPoints-d < 0)
            d = info.healthPoints;

        info.healthPoints -= d;
        timeStamp = Time.time + invbtyTime;
    }


    private void OnTriggerStay(Collider other) {
        int d = GetDamage(other.gameObject.tag);
        if(d > 0)
        {
            TakeDamage(d);
        }
    }

    private int GetDamage(string tag)
    {
        /*
            Para escolher o dano que cada coisa vai causar 
        */
        switch(tag)
        {
            /*
            case "Enemy":
                return 10;
            case "Boss":
                return 40;
            */
            case "Projectile":
                return 10;
            case "Trap":
                return 25;
            default:
                return -1;
        }
    }
}
