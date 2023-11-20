using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    public float fadeSpeed, fadeAmount;
    float originalOpacity;
    [SerializeField] Material[] materials;
    public bool isHidingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        materials = GetComponent<Renderer>().materials;
        foreach (Material material in materials)
        {
            originalOpacity = material.color.a;
        } 
        // Fade();
    }

    // Update is called once per frame
    void Update()
    {
        // if(isHidingPlayer)
        // {
        //     Fade();
        // }
        // else
        // {
        //     FadeOut();
        // }
    }


    public void Fade()
    {
        Debug.Log("FadeIn");
        foreach (Material material in materials)
        {
            Color color = material.color;
            color.a = Mathf.Lerp(color.a, fadeAmount, fadeSpeed * Time.deltaTime);
            material.color = color;
        }
    }

    public void FadeOut()
    {
        foreach (Material material in materials)
        {
            Color color = material.color;
            color.a = Mathf.Lerp(color.a, originalOpacity, fadeSpeed * Time.deltaTime);
            material.color = color;
        }
    }
}
