using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CinemachineManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] CinemachineVirtualCamera skillTreeCamera;
    private bool cameraIsOnPlayer = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) 
        {
            ChangeCamera();
        }
    }

    private void ChangeCamera()
    {
        if(cameraIsOnPlayer)
        {
            playerCamera.Priority = 0;
            skillTreeCamera.Priority = 1;
        }
        else
        {
            playerCamera.Priority = 1;
            skillTreeCamera.Priority = 0;
        }
        cameraIsOnPlayer = !cameraIsOnPlayer;
    }
}
