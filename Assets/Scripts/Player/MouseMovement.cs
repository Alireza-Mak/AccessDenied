using UnityEngine;

public enum RotationAxes
{
    MouseXAndY,
    MouseX,
    MouseY
}

public class MouseMovement : ActiveDuringGameplay
{
    [Header("Rotation Settings")]
    [SerializeField] private RotationAxes rotationAxes = RotationAxes.MouseXAndY;

    [Header("Sensitivity")]
    public float horizontalSensitivity = 9.0f;
    public float verticalSensitivity = 9.0f;

    [Header("Vertical Rotation Limits")]
    public float minVerticalAngle = -45.0f;
    public float maxVerticalAngle = 45.0f;

    private float verticalRotation = 0.0f;

    void Update()
    {
        if (rotationAxes == RotationAxes.MouseX)
        {
            float horizontalInput = Input.GetAxis("Mouse X") * horizontalSensitivity;
            transform.Rotate(Vector3.up * horizontalInput);
        }
        else if (rotationAxes == RotationAxes.MouseY)
        {
            verticalRotation -= Input.GetAxis("Mouse Y") * verticalSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);
            transform.localEulerAngles = new Vector3(verticalRotation, 0, 0);
        }
        else
        {
            verticalRotation -= Input.GetAxis("Mouse Y") * verticalSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

            float horizontalDelta = Input.GetAxis("Mouse X") * horizontalSensitivity;
            float currentYRotation = transform.localEulerAngles.y + horizontalDelta;

            transform.localEulerAngles = new Vector3(verticalRotation, currentYRotation, 0);
        }
    }

    public void ChangeSensitivity(float rate)
    {
        horizontalSensitivity /= rate;
        verticalSensitivity /= rate;
    }
}