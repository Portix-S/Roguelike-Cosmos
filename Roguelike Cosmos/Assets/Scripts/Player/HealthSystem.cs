
using UnityEngine;
using Player;

public class HealthSystem : MonoBehaviour
{
    public PlayerData info; // Informações do player
    private float invbtyTime = 0.5f; // Tempo de invencibilidade após receber dano
    private float timeStamp; // Registra o tempo que o player vai poder levar dano novamente
    private float maxHealth; // Vida máxima

    [SerializeField]
    private float actualHealth;

    [SerializeField]
    private HealthBar healthBar;
    void Start()
    {
        timeStamp = 0;
        //maxHealth = info.HealthPoints;
        healthBar.SetSlider();
        healthBar.SetMaxHealth(maxHealth);
        actualHealth = maxHealth;
        healthBar.SetHealth(maxHealth);

        //Debug.Log(info.modifier[0].value);
    }


    void Cooldown()
    {

    }

    private void Update() {
        if(Input.GetKeyDown("b")) TakeDamage(10f);
        if(Input.GetKeyDown("v")) Heal(20f);
            
    }

    public void Heal(float h) 
    {
        /*
            Para curar o player
        */
        if(actualHealth+h > maxHealth)
            h = maxHealth - actualHealth;
        

        actualHealth += h;
        healthBar.SetHealth(actualHealth);
    }

    void SetMaxHealth(float h)
    {
        if(h<=0) return;

        maxHealth = h;
        healthBar.SetMaxHealth(actualHealth);
    }

    public void TakeDamage(float d) 
    {
        /*
            Para tomar dano e atribui o tempo que o player 
            vai poder receber dano de novo.
            Pode ser chamada pelo objeto que vai dar dano
            na hora do contato.
        */
        if(timeStamp > Time.time) return;

        if(actualHealth-d < 0)
            d = maxHealth;

        actualHealth -= d;
        healthBar.SetHealth(actualHealth);
        timeStamp = Time.time + invbtyTime;
        if (actualHealth - d <= 0) ;
        //FAZER ALGUMA COISA
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
