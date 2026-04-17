using UnityEngine;

class Health : CollectableItem
{
    void Update()
    {
        transform.Rotate(Time.deltaTime * 180 * Vector3.up);
    }
}