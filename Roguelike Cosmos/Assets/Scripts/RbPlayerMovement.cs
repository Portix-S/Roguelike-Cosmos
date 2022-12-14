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
    [SerializeField] float dashDistance;

    bool dashing;
    bool onDashCooldown;

    [Header("Rotation")]
    [SerializeField] float smoothTurnTime = 0.1f;
    float smoothTurnVelocity;

    [Header("Animation")]
    public Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        //playerRb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();

        if (Input.GetKeyDown(KeyCode.LeftShift) && !onDashCooldown)
            dashing = true;
    }

    void FixedUpdate()
    {
        if (!dashing)
            MovePlayer();
        else
            StartCoroutine(Dash());
        Rotate();

        
    }

    private void MyInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MovePlayer()
    {
        if (Mathf.Abs(input.x) > Mathf.Epsilon || Mathf.Abs(input.y) > Mathf.Epsilon)
            playerAnimator.SetBool("isRunning", true);
        else
            playerAnimator.SetBool("isRunning", false);
        moveDirection = new Vector3(input.x, 0f, input.y).normalized;
        playerRb.AddForce(moveDirection * moveSpeed * 100f * Time.deltaTime, ForceMode.Force);
    }

    private void Rotate()
    {
        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(-moveDirection.z, moveDirection.x) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    IEnumerator Dash()
    {
        if (input.x != 0f || input.y != 0f)
        {
            dashing = true;
            playerAnimator.SetBool("isDashing", true);
            onDashCooldown = true;
            StartCoroutine(DashCooldown());
            if (Mathf.Abs(input.x) > Mathf.Epsilon && Mathf.Abs(input.y) > Mathf.Epsilon)
                moveDirection = new Vector3(Mathf.Sign(input.x) / 1.4125f, 0f, Mathf.Sign(input.y) / 1.4125f).normalized;
            else if (input.x != 0f && input.y != 0f)
                moveDirection = new Vector3(Mathf.Sign(input.x), 0f, Mathf.Sign(input.y)).normalized;
            else
                moveDirection = new Vector3(input.x, 0f, input.y).normalized;
        }
        
        
        playerRb.AddForce(moveDirection * dashDistance * 100f * Time.deltaTime, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        playerAnimator.SetBool("isDashing", false);
        dashing = false;
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(1f);
        onDashCooldown = false;
    }
}
