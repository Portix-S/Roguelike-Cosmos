using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleController : MonoBehaviour
{
    [SerializeField] private GameObject magicCircle;
    private ParticleSystem[] ps;

    private void Start() {
        ps = magicCircle.GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //magicCircle.SetActive(true);
            
            foreach (ParticleSystem p in ps)
            {
                p.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            //magicCircle.SetActive(false);
            foreach (ParticleSystem p in ps)
            {
                p.Stop();
            }
        }
    }
}
