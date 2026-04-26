using UnityEngine;

public enum WeaponType
{
    Pistol,
    Sniper,
    Knife
}

public abstract class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] protected WeaponType weaponType;
    [SerializeField] protected int maxAmmo = 20;

    [Header("Visual Effects")]
    [SerializeField] GameObject shellPrefab;
    [SerializeField] GameObject bulletPrefab;

    [Header("Cooldown Settings")]
    [SerializeField] protected float fireCooldown = 1f;
    private float lastFireTime = -Mathf.Infinity;

    public Transform FireSpawnPoint { get; private set; }
    public Transform ShellSpawnPoint { get; private set; }
    protected Camera playerCamera;
    public int CurrentNumbOfAmmo { get; private set; }
    protected bool isEquipped;


    private void Awake()
    {
        CurrentNumbOfAmmo = maxAmmo;

        Messenger<int>.AddListener(GameEvent.PICKUP_AMMO, OnIncreaseAmmo);
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        ShellSpawnPoint = transform.Find("ShellSpawnPoint");
        FireSpawnPoint = transform.Find("FireSpawnPoint");
    }

    private void Start()
    {
        if (isEquipped)
        {
            BroadcastAmmo();
        }
    }

    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.PICKUP_AMMO, OnIncreaseAmmo);
    }

    private void Fire()
    {
        DecreaseAmmo();

        shellPrefab.GetComponent<ShellController>().SpawnShell(ShellSpawnPoint);

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(
            playerCamera.pixelWidth / 2,
            playerCamera.pixelHeight / 2,
            0f
            ));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Messenger<RaycastHit>.Broadcast(GameEvent.PLAYER_BULLET_HIT, hit);
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }
            else
            {
                if (hit.collider.isTrigger == true) return;
                Quaternion rotation = Quaternion.LookRotation(ray.direction) * Quaternion.Euler(90f, 0f, 0f);
                float embedAmount = 0.05f;
                Vector3 spawnPosition = hit.point + ray.direction * embedAmount;
                GameObject bulletInstance = Instantiate(bulletPrefab, spawnPosition, rotation);
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null && !rb.isKinematic)
                {
                    float force = 3f; // adjust based on feel

                    // Apply impact force at the exact hit point
                    rb.AddForceAtPosition(ray.direction * force, hit.point, ForceMode.Impulse);

                    // Optional: add a bit of spin for realism
                    rb.AddTorque(Random.insideUnitSphere * 50f, ForceMode.Impulse);
                    Destroy(bulletInstance, 0.05f);
                    return;
                }
                Destroy(bulletInstance, 5f);
            }
        }
    }

    private void OnIncreaseAmmo(int amount)
    {
        CurrentNumbOfAmmo = amount + CurrentNumbOfAmmo > maxAmmo ? maxAmmo : amount + CurrentNumbOfAmmo;
        BroadcastAmmo();
    }

    private void DecreaseAmmo()
    {
        CurrentNumbOfAmmo--;
        BroadcastAmmo();
    }

    private void BroadcastAmmo()
    {
        Messenger<float, float>.Broadcast(GameEvent.AMMO_CHANGED, CurrentNumbOfAmmo, maxAmmo);
    }

    public virtual void OnPrimaryActionDown()
    {
        if (CurrentNumbOfAmmo <= 0) return;
        Cooldown();
        Fire();
    }

    protected void Cooldown()
    {
        if (Time.time < lastFireTime + fireCooldown)
            return;

        lastFireTime = Time.time;
    }

    public bool CanFire()
    {
        return Time.time >= lastFireTime + fireCooldown;
    }


    public virtual void OnSecondaryActionDown() { }

    public virtual void Unequip()
    {
        isEquipped = false;
        gameObject.SetActive(false);
    }

    public virtual void Equip()
    {
        isEquipped = true;
        gameObject.SetActive(true);
        BroadcastAmmo();
    }

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }
    public void OnWeaponChanged()
    {
        if (isEquipped)
        {
            BroadcastAmmo();
        }
    }

    public void PlayMuzzleFlash()
    {
        if (weaponType == WeaponType.Knife || CurrentNumbOfAmmo <= 0) return;
        FireSpawnPoint.GetChild(0).GetComponent<ParticleSystem>().Play();
    }
}