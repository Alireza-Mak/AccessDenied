using UnityEngine;

class Ammo : CollectableItem
{
    [Header("Detection")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Transform ammoBoxLid;

    [Header("Light")]
    [SerializeField] private Light pointLight;
    [SerializeField] private float lightOnIntensity = 5f;
    [SerializeField] private float lightOffIntensity = 0f;
    [SerializeField] private float lightSmoothSpeed = 5f;

    private bool isPlayerDetected;

    private Quaternion openRotation;
    private Quaternion closedRotation;

    void Start()
    {
        closedRotation = ammoBoxLid.rotation;
        openRotation = Quaternion.Euler(
          -180f,
          ammoBoxLid.eulerAngles.y,
          ammoBoxLid.eulerAngles.z
      );

    }
    void Update()
    {
        isPlayerDetected = Physics.CheckSphere(transform.position, detectionRadius, playerLayerMask);

        Quaternion target = isPlayerDetected ? openRotation : closedRotation;

        ammoBoxLid.rotation = Quaternion.Slerp(
            ammoBoxLid.rotation,
            target,
            rotationSpeed * Time.deltaTime
        );

        HandleLight();
    }

    void HandleLight()
    {
        if (pointLight != null)
        {
            float targetIntensity = isPlayerDetected ? lightOnIntensity : lightOffIntensity;
            float currentLightIntensity = Mathf.Lerp(pointLight.intensity, targetIntensity, lightSmoothSpeed * Time.deltaTime);
            pointLight.intensity = currentLightIntensity;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
