using System.Collections;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    [SerializeField] private float shellEjectionForce = 20f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float forwardRatio = 0.1f;
    [SerializeField] private float upRatio = -2f;

    public void SpawnShell(Transform spawnPoint)
    {
        Quaternion rot = spawnPoint.rotation * Quaternion.Euler(0f, 0f, -90f);
        GameObject shellInstance = Instantiate(gameObject, spawnPoint.position, rot);
        Rigidbody shellRigidbody = shellInstance.GetComponent<Rigidbody>();

        Vector3 ejectionDirection =
            spawnPoint.forward * forwardRatio + spawnPoint.up * upRatio;

        shellRigidbody.AddForce(ejectionDirection * shellEjectionForce, ForceMode.Impulse);

        Destroy(shellRigidbody, lifeTime);
    }
}