using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RbPlayerMovement : MonoBehaviour
{
    Vector3 moveDirection;
    Vector2 input;
    Rigidbody playerRb;

    [Header("Movement")]
    [SerializeField] float moveSpeed;

    [SerializeField] float groundDrag;

    [Header("Ground Check")] // Utiliza a altura do player para ver se está no chão com RayCast
    public float playerHeight;
    public bool grounded;
    public LayerMask whatIsGround;  
    

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        MovePlayer();


    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


        grounded = Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0), Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (grounded)
            playerRb.drag = groundDrag;
        else
            playerRb.drag = 0f;

    }

    private void MovePlayer()
    {
        moveDirection = new Vector3(-input.x, 0f, -input.y);

        playerRb.AddForce(moveDirection * moveSpeed * 100f * Time.deltaTime, ForceMode.Force);
    }
}
