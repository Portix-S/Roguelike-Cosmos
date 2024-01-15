using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public PlayerData info; // Informações do player
    private float invbtyTime = 0.5f; // Tempo de invencibilidade após receber dano
    private float timeStamp; // Registra o tempo que o player vai poder levar dano novamente
    private float maxHealth; // Vida máxima
    private float health; // Vida máxima

    [SerializeField] GameObject transicao;
    private NavMeshAgent playerNavMeshAgent;
    private Plane plane;
    private RbPlayerMovement rbPlayerMovement;
    public Transform warpPoint;
    [SerializeField] Image healthSlider;
    
    // Vignette postprocess
    [SerializeField] PostProcessProfile profile;
    [SerializeField] private Vignette vignette;
    private float vignetteTime = 0.3f;
    private float vignetteTimer = 0f;
    private bool fadingIn = false;
    private bool fadingOut = false;
    void Start()
    {
        timeStamp = 0;
        UpdateStats();
        //info.baseHealthPoints = maxHealth;
        plane = new Plane(Vector3.up, Vector3.zero);
        rbPlayerMovement = GetComponent<RbPlayerMovement>();
        playerNavMeshAgent = GetComponent<NavMeshAgent>();
        healthSlider.fillAmount = 1;
        //transicao = GameObject.FindGameObjectWithTag("Transicao");
        profile.TryGetSettings(out vignette);
        vignette.intensity.value = 0f;
    }


    void Cooldown()
    {

    }

    public void UpdateStats()
    {
        health = (int)info.HealthPoints;
        maxHealth = health;
        healthSlider.fillAmount = 1;
    }

    private void Update()
    {
        if (fadingIn || fadingOut)
        {
            vignetteTimer += Time.deltaTime;
            float t = vignetteTimer / vignetteTime;
            if (fadingOut)
            {
                vignette.intensity.value = Mathf.Lerp(0.5f, 0f, t);
            }
            else
                vignette.intensity.value = Mathf.Lerp(0f, 0.5f, t*2f);
            
            if(vignetteTimer >= vignetteTime)
            {
                fadingIn = false;
                fadingOut = false;
                vignetteTimer = 0f;
            }
        }
        
        //Only on Unity Editor
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.N))
            StartCoroutine(Morrer());

        #endif

    }

    public void Heal(float h) 
    {
        /*
            Para curar o player
        */
        vignetteTimer = 0f;

        if(health + h > maxHealth)
            h = maxHealth - health;


        health += h;
        healthSlider.fillAmount = (float)health / (float)maxHealth;
        // vignetteTime = 0.8f;
        // vignette.color.value = Color.green;
        // vignette.active = true;
        // fadingIn = true;
        // StartCoroutine("EndDamageVignette");
        VignnetteEffect(0.8f, Color.green);
    }

    void SetMaxHealth(float h)
    {
        if (h > 0)
        {
            maxHealth = h;
            health = maxHealth;
        }
    }

    public void TakeDamage(float d) // Trocar para receber dano físico e mágico
    {
        /*
            Para tomar dano e atribui o tempo que o player 
            vai poder receber dano de novo.
            Pode ser chamada pelo objeto que vai dar dano
            na hora do contato.
        */
        if(timeStamp > Time.time) return;
        float random = UnityEngine.Random.Range(0, 100);
        float dodge = info.Dodge * 2f;
        dodge = Mathf.Clamp(dodge, 0, 70f);
        Debug.Log("Dodge: " + dodge);
        Debug.Log("Armor: " + info.Armor);

        if (random > dodge)
        {
            vignetteTimer = 0f;
            if (health - d < 0)
                d = health;
            
            float percentageDamageReduction = (info.Armor * 2f / 100); 
            health -= (d - (d * percentageDamageReduction));
            Debug.Log("Current Health: " + health);
            timeStamp = Time.time + invbtyTime;
            healthSlider.fillAmount = (float)health / (float)maxHealth;
            
            //Vignette
            VignnetteEffect(0.3f, Color.red);
        }
        else
        {
            Debug.Log("Dodge");
            VignnetteEffect(0.8f, Color.cyan);
            timeStamp = Time.time + invbtyTime;
        }

        // Morrer
        if(health <= 0){
            healthSlider.fillAmount = 0;
            StartCoroutine(Morrer());
        }
    }

    private void VignnetteEffect(float time, Color color)
    {
        vignetteTime = time;
        vignette.color.value = color;
        vignette.active = true;
        fadingIn = true;
        StartCoroutine("EndDamageVignette");
    }
    
    IEnumerator EndDamageVignette()
    {
        yield return new WaitForSeconds(vignetteTime);
        // vignette.active = false;
        fadingIn = false;
        fadingOut = true;

        // vignette.intensity = new FloatParameter { value = 0f };
        // vignette.intensity.overrideState = false;
    }

    public IEnumerator Morrer(){
        //Time.timeScale = 0f;
        transicao.SetActive(true);
        WaveManager[] wms = FindObjectsOfType<WaveManager>();
        foreach(WaveManager wm in wms)
        {
            wm.KillAllEnemies();
        }
        rbPlayerMovement.enabled = false;
        yield return new WaitForSeconds(1.8f);
        playerNavMeshAgent.enabled = false;
        transform.position = plane.ClosestPointOnPlane(warpPoint.position);
        playerNavMeshAgent.enabled = true;
        rbPlayerMovement.enabled = true;
        UpdateStats();
        //SceneManager.LoadScene("GameOver");
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
