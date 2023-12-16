using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using TMPro;
=======
>>>>>>> abdaf47560c4faf640cac4a749718702198f89c2
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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
    [SerializeField] bool isEndWarp; //Temporário
<<<<<<< HEAD
    GameManager gm;
    private void Start() {
        //canvasTransition = GameObject.FindGameObjectWithTag("Transicao");
        canvasTransition.SetActive(false);
        canWarp = false;
        plane = new Plane(Vector3.up, Vector3.zero);
        gm = FindObjectOfType<GameManager>();
=======
    private void Start() {
        canvasTransition = GameObject.FindGameObjectWithTag("Transicao");
        canvasTransition.SetActive(false);
        canWarp = false;
        plane = new Plane(Vector3.up, Vector3.zero);
>>>>>>> abdaf47560c4faf640cac4a749718702198f89c2
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
        {
<<<<<<< HEAD
            canWarp = (wm.currentState == WaveManager.WaveState.ENDED);
=======
>>>>>>> abdaf47560c4faf640cac4a749718702198f89c2
            if(canWarp && isEndWarp)
            {
                SceneManager.LoadScene("EndGame");
            }
<<<<<<< HEAD
            playerPos = other.GetComponent<Transform>();
            playerNavMeshAgent = other.GetComponent<NavMeshAgent>();
            rbPlayerMovement = other.GetComponent<RbPlayerMovement>();
            gm.canOpenSkillTree = false;
            rbPlayerMovement.StopPlayer();
=======
            canWarp = (wm.currentState == WaveManager.WaveState.ENDED);
            playerPos = other.GetComponent<Transform>();
            playerNavMeshAgent = other.GetComponent<NavMeshAgent>();
            rbPlayerMovement = other.GetComponent<RbPlayerMovement>();
>>>>>>> abdaf47560c4faf640cac4a749718702198f89c2
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
