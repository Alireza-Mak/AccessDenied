using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private Animator anim;

    [Header("Jump")]
    [Range(0.1f, 5.0f)]
    [SerializeField] private float jumpHeight = 1.0f;

    [Range(0.1f, 5.0f)]
    [SerializeField] private float jumpTimeToApex = 0.3f;

    [SerializeField] private int maximumNumberOfJumps = 1;

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;

    private float gravity;
    private float yVelocity = 0.0f;
    private float yVelocityWhenGrounded = -4.0f;
    private float initialJumpVelocity;
    private int numOfJumps = 0;

    void Start()
    {
        initialJumpVelocity = (2 * jumpHeight) / jumpTimeToApex;
        gravity = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
    }

    void Update()
    {
        HandleJump();
        HandleMovement();
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            numOfJumps++;
            if (numOfJumps <= maximumNumberOfJumps)
            {
                yVelocity = initialJumpVelocity;
                anim.SetTrigger("jump");
            }
        }

        if (cc.isGrounded && yVelocity < 0.0f)
        {
            yVelocity = yVelocityWhenGrounded;
            numOfJumps = 0;
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }
    }


    private void HandleMovement()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizInput, 0f, vertInput);

        Vector3.ClampMagnitude(movement, 1.0f);

        bool isRunning = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        movement = transform.TransformDirection(movement);

        movement.y = yVelocity;
        movement *= currentSpeed;
        movement *= Time.deltaTime;

        cc.Move(movement);

        float horizontalSpeed = new Vector3(cc.velocity.x, 0f, cc.velocity.z).magnitude;

        anim.SetFloat("velocity", cc.velocity.magnitude);

        float animVelocity = 0f;
        if (runSpeed > 0f)
        {
            animVelocity = horizontalSpeed / runSpeed;
        }

        animVelocity = Mathf.Clamp01(animVelocity);

        anim.SetFloat("velocity", animVelocity);
    }
}
