using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    // This Player Movement uses NavMesh

    NavMeshAgent navMeshAgent;
    public Transform fixedTransform;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 destination = fixedTransform.position + fixedTransform.right * input.x + fixedTransform.forward * input.y;
        navMeshAgent.destination = destination;
    }
}
