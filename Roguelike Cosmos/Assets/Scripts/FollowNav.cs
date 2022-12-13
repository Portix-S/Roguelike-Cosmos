using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNav : MonoBehaviour
{

    public Transform playerPos;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = playerPos.position;
    }
}
