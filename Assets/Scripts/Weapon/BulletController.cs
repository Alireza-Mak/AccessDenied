using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;

    public void SpawnBullet(Vector3 position, Vector3 dir)
    {
        Quaternion rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(90f, 0f, 0f);
        GameObject bulletInstance = Instantiate(gameObject, position, rotation);
        Destroy(bulletInstance, lifeTime);
    }
}