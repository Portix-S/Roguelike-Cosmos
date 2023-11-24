using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warp : MonoBehaviour
{
    public Transform warpPoint;
    public Transform playerPos;
    public WaveManager wm;
    [HideInInspector] public bool canWarp;
    private WaveManager newRoomWm;
    public GameObject canvasTransition;
    private RbPlayerMovement rbPlayerMovement;
    private Plane plane;
    NavMeshAgent playerNavMeshAgent;
    [SerializeField] bool isSpawnWarp;
    private void Start() {
        canvasTransition = GameObject.FindGameObjectWithTag("Transicao");
        canvasTransition.SetActive(false);
        canWarp = false;
        plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
        {
            canWarp = (wm.currentState == WaveManager.WaveState.ENDED);
            playerPos = other.GetComponent<Transform>();
            playerNavMeshAgent = other.GetComponent<NavMeshAgent>();
            rbPlayerMovement = other.GetComponent<RbPlayerMovement>();
            if(canWarp || isSpawnWarp)
            {
                rbPlayerMovement.enabled = false;
                StartCoroutine(RunTransition());
            }
        }
        //canWarp = true; // For testing
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player")
            canWarp = false;
    }


    IEnumerator RunTransition(){
        canvasTransition.SetActive(true);
        yield return new WaitForSeconds(1.9f);
        wm.enabled = false;
        newRoomWm = warpPoint.GetComponent<WaveManager>();
        newRoomWm.enabled = true;
        newRoomWm.Restart();
        playerNavMeshAgent.enabled = false;
        playerPos.position = plane.ClosestPointOnPlane(warpPoint.position);
        playerNavMeshAgent.enabled = true;
        rbPlayerMovement.enabled = true;

    }
}
