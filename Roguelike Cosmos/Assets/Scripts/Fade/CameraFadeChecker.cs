using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFadeChecker : MonoBehaviour
{
    [SerializeField] private ObjectFader objectFader;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = player.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            if(hit.collider.gameObject.tag == "Player")
            {
                if(objectFader != null)
                {
                    // objectFader.FadeOut();
                    objectFader.isHidingPlayer = false;
                }
            }
            else
            {
                objectFader = hit.collider.gameObject.GetComponent<ObjectFader>();
                if(objectFader != null)
                {
                    // objectFader.Fade();
                    objectFader.isHidingPlayer = true;
                    Debug.Log("Fade");
                }
            }
        }
    }
}
