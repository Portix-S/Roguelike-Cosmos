using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleController : MonoBehaviour
{
    [SerializeField] private GameObject magicCircle;
    private ParticleSystem[] ps;
    GameManager gm;
    
    [SerializeField] private GameObject skillTreeButton;
    private void Start() {
        ps = magicCircle.GetComponentsInChildren<ParticleSystem>();
        gm = FindObjectOfType<GameManager>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //magicCircle.SetActive(true);
            if(SystemInfo.deviceType == DeviceType.Handheld)
                skillTreeButton.SetActive(true);
            gm.canOpenSkillTree = true;
            foreach (ParticleSystem p in ps)
            {
                p.Play();
            }
        }
    }


    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            gm.canOpenSkillTree = false;
            skillTreeButton.SetActive(false);
            //magicCircle.SetActive(false);
            foreach (ParticleSystem p in ps)
            {
                p.Stop();
            }
<<<<<<< HEAD
            // gm.OpenSkillTree(false)
=======
>>>>>>> abdaf47560c4faf640cac4a749718702198f89c2
        }
    }
}
