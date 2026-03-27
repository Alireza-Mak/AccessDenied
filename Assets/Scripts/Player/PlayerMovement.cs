using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController charController;
    [SerializeField]  private Animator anim;

    private float speed = 9.0f;
    private float gravity = -9.81f;
    void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizInput, 0, vertInput);

        Vector3.ClampMagnitude(movement, 1.0f);

        movement.y = gravity;
        movement *= speed;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        charController.Move(movement);
        anim.SetFloat("Velocity", charController.velocity.magnitude);
    }
}
