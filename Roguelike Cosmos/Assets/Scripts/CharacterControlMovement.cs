using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlMovement : MonoBehaviour
{
    public CharacterController controller;

    [SerializeField] float speed = 12f;


    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 move = new Vector3(-input.x, 0f, -input.y);

        controller.Move(move * speed * Time.deltaTime);
    }
}
