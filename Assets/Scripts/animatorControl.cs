using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public Transform weaponHolder;
    public List<GameObject> weapons = new List<GameObject>();

    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float rotationSpeed = 10f;

    private Vector3 velocity;
    private bool isRunning = false;
    private int currentWeaponIndex = -1;
    private int clickCounter = 0;
    private float clickResetTime = 1.5f;

    private PlayerInput playerInput;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool attackPressed;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        UpdateWeaponState();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleWeaponSwitching();
        HandleAttacking();
    }

    private void HandleMovement()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        isRunning = playerInput.actions["Sprint"].ReadValue<float>() > 0;

        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move.y = 0;
        controller.Move(move.normalized * speed * Time.deltaTime);

        // Apply Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // **Rotation Fix**
        if (moveInput.y > 0 || (moveInput.x != 0 && moveInput.y != 0))
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // **Animation Updates**
        animator.SetBool("IsWalking", moveInput.y > 0);
        animator.SetBool("IsRunning", isRunning && moveInput.y > 0);
        animator.SetBool("IsBackwarding", moveInput.y < 0);
        animator.SetFloat("BackwardSpeed", (moveInput.y < 0 && isRunning) ? speed : 0);
        animator.SetBool("IsStrafingLeft", moveInput.x < 0 && moveInput.y == 0);
        animator.SetBool("IsStrafingRight", moveInput.x > 0 && moveInput.y == 0);
    }

    private void HandleJump()
    {
        jumpPressed = playerInput.actions["Jump"].WasPressedThisFrame();

        if (jumpPressed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("IsJumping", true);
        }

        if (controller.isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    private void HandleWeaponSwitching()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (playerInput.actions["SwitchWeapon" + (i + 1)].WasPressedThisFrame())
            {
                currentWeaponIndex = (currentWeaponIndex == i) ? -1 : i;
                UpdateWeaponState();
            }
        }
    }

    private void UpdateWeaponState()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(i == currentWeaponIndex);
        }

        bool hasWeapon = currentWeaponIndex != -1;
        animator.SetBool("IsWeaponEquipped", hasWeapon);
        clickCounter = 0;
        animator.SetInteger("ClickCounter", clickCounter);
    }

    private void HandleAttacking()
    {
        attackPressed = playerInput.actions["Attack"].WasPressedThisFrame();

        if (currentWeaponIndex != -1 && attackPressed)
        {
            clickCounter = (clickCounter % 3) + 1;
            animator.SetInteger("ClickCounter", clickCounter);

            CancelInvoke(nameof(ResetClickCounter));
            Invoke(nameof(ResetClickCounter), clickResetTime);
        }
    }

    private void ResetClickCounter()
    {
        clickCounter = 0;
        animator.SetInteger("ClickCounter", clickCounter);
    }
}
