using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    // This Player Movement uses NavMesh

    NavMeshAgent navMeshAgent;
    public Transform fixedTransform;
    [SerializeField] float dashDistance;
    float normalSpeed;
    float normalAccel;
    public Vector2 input;
    bool dashing;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        normalSpeed = navMeshAgent.speed;
        normalAccel = navMeshAgent.acceleration;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused) return;

        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        

        if(Input.GetKeyDown(KeyCode.LeftShift) && !dashing)
        {
            StartCoroutine(NormalSpeed(input.x, input.y));
        }
        else if(!dashing)
        {
            Vector3 destination = fixedTransform.position + fixedTransform.right * input.x + fixedTransform.forward * input.y;
            navMeshAgent.destination = destination;
            //navMeshAgent.Move(destination);
        }
    }

    private void Dash()
    {


        /*
        float xMag = 1f;
        float yMag = 1f;
        if (Mathf.Abs(input.x) > Mathf.Epsilon && Mathf.Abs(input.y) > Mathf.Epsilon)
        {
            if (input.x < 0)
                xMag = -0.5f;
            if (input.y < 0)
                yMag = -0.5f;
            StartCoroutine(NormalSpeed(xMag, yMag));
        }
        else if (Mathf.Abs(input.x) > Mathf.Epsilon)
        {
            if (input.x < 0)
                xMag = -1f;
            StartCoroutine(NormalSpeed(xMag, 0f));
        }
        else if (Mathf.Abs(input.y) > Mathf.Epsilon)
        {
            if (input.y < 0)
                yMag = -1f;
            StartCoroutine(NormalSpeed(0f, yMag));
        }
        //*/
    }

    IEnumerator NormalSpeed(float xMag, float yMag)
    {
        dashing = true;
        Vector3 dashDestination = new Vector3();
        navMeshAgent.speed = 130f;
        navMeshAgent.acceleration = 500f;
        dashDestination = fixedTransform.position + transform.forward * dashDistance;
        //dashDestination = transform.forward * dashDistance * xMag * yMag;
        navMeshAgent.destination = dashDestination;
        //navMeshAgent.Move(dashDestination);
        yield return new WaitForSeconds(0.1f);
        navMeshAgent.speed = normalSpeed;
        navMeshAgent.acceleration = normalAccel;
        dashing = false;
    }

}
