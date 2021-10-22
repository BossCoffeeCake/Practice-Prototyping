using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionReference movementControl;
    [SerializeField] InputActionReference jumpControl;
    [SerializeField] float playerSpeed; //basic is 2.0f;
    [SerializeField] float jumpHeight; //basic is 1.0f;
    [SerializeField] float rotationSpeed; //basic is 4f;

    const float gravityValue = -20f;

    CharacterController controller;
    Vector3 playerVelocity;
    bool groundedPlayer;
    Transform cameraMainTransform;
    Animator animator;
    int frame;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    //Samyam said we need enable & disable because we don't have an input controller
    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
    }

    void Update()
    {
        Animate();
        Movement();
    }

    void Movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player.
        if (jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    void Animate()
    {
        Vector2 movement = movementControl.action.ReadValue<Vector2>();

        if (jumpControl.action.triggered && controller.isGrounded)
        {
            animator.SetBool("isJumping", true);
            StartCoroutine(Jump());
        }

        if (controller.isGrounded)
        {
            //animator.SetBool("isJumping", false);
            if (movement.magnitude > .2f)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
    }
    IEnumerator Jump()
    {
        while (controller.isGrounded)
        {
            yield return new WaitForFixedUpdate();
        }
        while (!controller.isGrounded)
        {
            yield return new WaitForFixedUpdate();
        }
        animator.SetBool("isJumping", false);
    }
}