//using UnityEngine;

//public class BulletController : MonoBehaviour
//{
//    [SerializeField] private float lifeTime = 5f;

//    public void SpawnBullet(Vector3 spawnPosition, Vector3 spawnDirection, bool distructionActivation = true)
//    {
//        Quaternion rotation = Quaternion.LookRotation(spawnDirection) * Quaternion.Euler(90f, 0f, 0f);
//        GameObject bulletInstance = Instantiate(gameObject, spawnPosition, rotation);
//        if (distructionActivation)
//        {
//            Destroy(bulletInstance, lifeTime);
//        }
//    }

//    public virtual void OnTriggerEnter(Collider other) { }
//}
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 6f;
    public float toNewtons = 100f;
    private Rigidbody rb;
    private Vector3 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 direction)
    {
        moveDirection = direction;
        rb.isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            Vector3 movement = speed * Time.deltaTime * toNewtons * moveDirection;

            rb.linearVelocity = movement;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.Hit();
        }
        Destroy(gameObject);
    }
}