using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class RbPlayerMovement : MonoBehaviour
{
    Vector3 moveDirection;
    Vector2 input;
    Rigidbody playerRb;
    public Joystick joystick;
    DeviceType system;
    bool isMobileDevice;


    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float dashDistance;
    float dashCooldownTimer = 1f;
    [SerializeField] float dashTime = 0.75f;

    public bool dashing;
    bool onDashCooldown;

    [Header("Rotation")]
    [SerializeField] float smoothTurnTime = 0.1f;
    float smoothTurnVelocity;

    [Header("Animation")]
    public Animator playerAnimator;
    PlayerCombat playerCombat;
    private bool isNotAttacking;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        system = SystemInfo.deviceType;
        //playerRb.freezeRotation = true;
        isMobileDevice = false;
        playerCombat = GetComponent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();

        if (Input.GetKeyDown(KeyCode.LeftShift) && !onDashCooldown && playerAnimator.GetBool("isRunning"))
        {
            dashing = true;
        }

        //Only for tests on PC
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeInput();
        }
    }

    public void ChangeInput()
    {
        isMobileDevice = !isMobileDevice;
        if (isMobileDevice)
            system = DeviceType.Handheld;
        else
            system = DeviceType.Desktop;
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
        if(system == DeviceType.Desktop)
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        else
            input = new Vector2(joystick.Horizontal, joystick.Vertical);
    }

    private void MovePlayer()
    {
        //bool hasMobileInput = Mathf.Abs(joystick.Horizontal) > Mathf.Epsilon || Mathf.Abs(joystick.Vertical) > Mathf.Epsilon;
        //bool hasPcInput = Mathf.Abs(input.x) > Mathf.Epsilon || Mathf.Abs(input.y) > Mathf.Epsilon;
        isNotAttacking = !playerCombat.isAttacking && !playerCombat.isShooting;
        if (((Mathf.Abs(input.x) > Mathf.Epsilon || Mathf.Abs(input.y) > Mathf.Epsilon)))
            playerAnimator.SetBool("isRunning", true);
        else
            playerAnimator.SetBool("isRunning", false);
        //if (hasPcInput) // mudar depois para system == DeviceType.Desktop 
            moveDirection = new Vector3(input.x, 0f, input.y).normalized;

        //else
           //moveDirection = new Vector3(joystick.Horizontal, 0f, joystick.Vertical).normalized;

        if(isNotAttacking) //moveDirection = Vector3.zero;
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
        if ((input.x != 0f || input.y != 0f))
        {
            dashing = true;
            playerAnimator.SetBool("isDashing", true);
            onDashCooldown = true;
            StartCoroutine(DashCooldown());
            if (Mathf.Abs(input.x) > Mathf.Epsilon && Mathf.Abs(input.y) > Mathf.Epsilon)   // Dashing Diagonally
                moveDirection = new Vector3(Mathf.Sign(input.x) / 1.4125f, 0f, Mathf.Sign(input.y) / 1.4125f).normalized;
            else if (input.x != 0f && input.y != 0f) // Dashing Straight
                moveDirection = new Vector3(Mathf.Sign(input.x), 0f, Mathf.Sign(input.y)).normalized;
        }
        
        
        playerRb.AddForce(moveDirection * dashDistance * 100f * Time.deltaTime, ForceMode.Impulse);
        yield return new WaitForSeconds(dashTime);
        playerAnimator.SetBool("isDashing", false);
        dashing = false;
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldownTimer);
        onDashCooldown = false;
    }
}
