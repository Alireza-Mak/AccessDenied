using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shellCasingPrefab;
    [SerializeField] private Transform shellCasingSpawnPoint;
    [SerializeField] private float shellEjectionForce = 2f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SpawnObject(
                shellCasingPrefab,
                shellCasingSpawnPoint.position,
                Quaternion.FromToRotation(Vector3.up, playerCamera.transform.forward),
                20f,
                true
            ));

            weaponAnimator.SetTrigger("shoot");

            Vector3 screenCenter = new Vector3(
                playerCamera.pixelWidth / 2,
                playerCamera.pixelHeight / 2,
                0
            );

            Ray ray = playerCamera.ScreenPointToRay(screenCenter);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hitObject = hit.transform.gameObject;

                if (hitObject.CompareTag("Enemy"))
                {
                    Debug.Log("Found Enemy");
                }
                else
                {
                    StartCoroutine(SpawnObject(
                        bulletPrefab,
                        hit.point,
                        Quaternion.FromToRotation(Vector3.up, ray.direction),
                        20f
                    ));
                }
            }
        }
    }

    IEnumerator SpawnObject(GameObject prefab, Vector3 spawnPoint, Quaternion rotation, float lifeTime, bool ejectShell = false)
    {
        GameObject spawnedObject = Instantiate(prefab, spawnPoint, rotation);

        if (ejectShell)
        {
            Rigidbody rigidbodyComponent = spawnedObject.GetComponent<Rigidbody>();

            if (rigidbodyComponent != null)
            {
                Vector3 ejectionDirection = playerCamera.transform.right + playerCamera.transform.up;
                rigidbodyComponent.AddForce(ejectionDirection * shellEjectionForce, ForceMode.Impulse);
            }
        }

        yield return new WaitForSeconds(lifeTime);
        Destroy(spawnedObject);
    }

    private void OnDrawGizmos()
    {
        if (playerCamera == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            playerCamera.transform.position,
            playerCamera.transform.position + playerCamera.transform.forward * 100f
        );
    }
}