using UnityEngine;
using System.Collections;

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


    public Transform FireSpawnPoint { get; private set; }
    public Transform ShellSpawnPoint { get; private set; }
    protected Camera playerCamera;
    protected int currentAmmo;
    protected bool isEquipped;


    private void Awake()
    {
        currentAmmo = maxAmmo;

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
            if (hit.collider.CompareTag("Enemy"))
            {
                Messenger.Broadcast(GameEvent.ENEMY_DEAD);
            }
            else
            {
                Quaternion rotation = Quaternion.LookRotation(ray.direction) * Quaternion.Euler(90f, 0f, 0f);
                GameObject bulletInstance = Instantiate(bulletPrefab, hit.point, rotation);
                Destroy(bulletInstance, 5f);
            }
        }
    }

    private void OnIncreaseAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        BroadcastAmmo();
    }

    private void DecreaseAmmo()
    {
        currentAmmo--;
        BroadcastAmmo();
    }

    private void BroadcastAmmo()
    {
        Messenger<float, float>.Broadcast(GameEvent.AMMO_CHANGED, currentAmmo, maxAmmo);
    }

    public virtual void OnPrimaryActionDown()
    {
        Fire();
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
        if (weaponType != WeaponType.Knife)
        {
            FireSpawnPoint.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
    }
}