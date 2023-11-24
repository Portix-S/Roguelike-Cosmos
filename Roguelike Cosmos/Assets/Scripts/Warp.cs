using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Transform warpPoint;
    public Transform playerPos;
    public WaveManager wm;
    [HideInInspector] public bool canWarp;

    public GameObject canvasTransition;

    private Plane plane;

    private void Start() {
        canvasTransition = GameObject.FindGameObjectWithTag("Transicao");
        canvasTransition.SetActive(false);
        canWarp = false;
        plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void OnTriggerEnter(Collider other) {
        canWarp = (wm.currentState == WaveManager.WaveState.ENDED);
        //canWarp = true; // For testing
    }

    private void OnTriggerExit(Collider other) {
        canWarp = false;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E) && canWarp){
            playerPos.position = plane.ClosestPointOnPlane(warpPoint.position);
            StartCoroutine(RunTransition());
        }
    }

    IEnumerator RunTransition(){
        canvasTransition.SetActive(true);
        yield return new WaitForSeconds(2f);
        canvasTransition.GetComponent<Animator>().SetTrigger("EndTransition");
        yield return new WaitForSeconds(2f);
        canvasTransition.SetActive(false);
    }
}
