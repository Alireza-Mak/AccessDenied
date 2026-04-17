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
    private static readonly string SHOOT = "shoot";
    [Header("Weapon Settings")]
    [SerializeField] protected WeaponType weaponType;
    [SerializeField] protected Animator weaponAnimator;
    [SerializeField] protected Camera playerCamera;

    [Header("Visual Effects")]
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected Transform shellSpawnPoint;
    [SerializeField] GameObject shellPrefab;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private float lifeTime = 5f;




    [Header("Ammo Settings")]
    [SerializeField] protected int maxAmmo = 20;


    protected int currentAmmo;
    protected bool isEquipped;


    void Awake()
    {
        currentAmmo = maxAmmo;

        Messenger<int>.AddListener(GameEvent.PICKUP_AMMO, OnIncreaseAmmo);
        Messenger.AddListener(GameEvent.SHOOT_FRAME, HandleShootFrame);
    }

    void Start()
    {
        if (isEquipped)
        {
            BroadcastAmmo();
        }
    }

    void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.PICKUP_AMMO, OnIncreaseAmmo);
        Messenger.RemoveListener(GameEvent.SHOOT_FRAME, HandleShootFrame);
    }
    public virtual void OnPrimaryActionDown()
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
            case WeaponType.Sniper:
                if (currentAmmo <= 0) { return; }
                PerformFire();
                break;

            case WeaponType.Knife:
                PerformMeleeAttack();
                break;
        }
    }

    public virtual void OnSecondaryActionDown() { }
    public virtual void PerformMeleeAttack() { }
    public virtual void PerformFire()
    {
        DecreaseAmmo();
        weaponAnimator.SetTrigger(SHOOT);


        shellPrefab.GetComponent<ShellController>().SpawnShell(shellSpawnPoint);

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(
            playerCamera.pixelWidth / 2,
            playerCamera.pixelHeight / 2,
            0f
            ));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit");
                // Add damage logic here later.
            }
            else
            {
                bulletPrefab.GetComponent<BulletController>().SpawnBullet(hit.point, ray.direction.normalized);
            }
        }
    }

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
    protected void BroadcastAmmo()
    {
        Messenger<float, float>.Broadcast(GameEvent.AMMO_CHHANGED, currentAmmo, maxAmmo);
    }

    protected void DecreaseAmmo()
    {
        currentAmmo--;
        BroadcastAmmo();
    }

    protected virtual void OnIncreaseAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        BroadcastAmmo();
    }


    public virtual void OnWeaponChanged()
    {
        if (isEquipped)
        {
            BroadcastAmmo();
        }
    }

    protected virtual void HandleShootFrame()
    {
        if (!isEquipped)
            return;

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }


}