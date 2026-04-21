using UnityEngine;

class Floppy : CollectableItem
{
    [SerializeField] float baseRotationSpeed = 60f;
    [SerializeField] float tiltAmount = 10f;
    [SerializeField] float tiltSpeed = 2f;

    [SerializeField] float floatAmplitude = 0.25f;
    [SerializeField] float floatSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        float time = Time.time;

        transform.Rotate(Time.deltaTime * baseRotationSpeed * Vector3.up);

        // tilt wobble (X & Z)
        float tiltX = Mathf.Sin(time * tiltSpeed) * tiltAmount;
        float tiltZ = Mathf.Cos(time * tiltSpeed) * tiltAmount;

        transform.rotation = Quaternion.Euler(tiltX, transform.rotation.eulerAngles.y, tiltZ);

        // Floating effect
        float newY = startPos.y + Mathf.Sin(time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

    }
}