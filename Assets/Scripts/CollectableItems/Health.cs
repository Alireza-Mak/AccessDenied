using UnityEngine;

class Health : CollectableItem
{
    [SerializeField] float baseRotationSpeed = 60f;
    void Update()
    {
        transform.Rotate(Time.deltaTime * baseRotationSpeed * Vector3.up);
    }
}