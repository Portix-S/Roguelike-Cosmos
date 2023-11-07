using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class HealthSystem : MonoBehaviour
{
    public PlayerData info; // Informações do player
    private float invbtyTime = 0.5f; // Tempo de invencibilidade após receber dano
    private float timeStamp; // Registra o tempo que o player vai poder levar dano novamente
    private int maxHealth; // Vida máxima
    private int health; // Vida máxima
    void Start()
    {
        timeStamp = 0;
        UpdateStats();
        //info.baseHealthPoints = maxHealth;

    }


    void Cooldown()
    {

    }

    public void UpdateStats()
    {
        health = (int)info.HealthPoints;
        maxHealth = health;
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
        if(health + h > maxHealth)
            h = maxHealth - health;


        health += h;
    }

    void SetMaxHealth(int h)
    {
        if (h > 0)
        {
            maxHealth = h;
            health = maxHealth;
        }
    }

    public void TakeDamage(int d) // Trocar para receber dano físico e mágico
    {
        /*
            Para tomar dano e atribui o tempo que o player 
            vai poder receber dano de novo.
            Pode ser chamada pelo objeto que vai dar dano
            na hora do contato.
        */
        if(timeStamp > Time.time) return;
        if (health - d < 0)
            d = health;
        health -= d;
        Debug.Log("Current Health: " + health);
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
