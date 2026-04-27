using UnityEngine;

public class PlayerCharacter : ActiveDuringGameplay
{
    private int health;
    public static readonly int maxHealth = 5;
    [SerializeField] private WeaponManager WeaponManager;
    private float pushForce = 5.0f;

    protected override void Awake()
    {
        base.Awake();
        Messenger<int>.AddListener(GameEvent.PICKUP_HEALTH, OnPickupHealth);
        health = maxHealth;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Fire = Left Mouse Button
        if (Input.GetMouseButtonDown(0))
        {
            WeaponManager.StartFiring();
        }

        // Fire = Right Mouse Button
        if (Input.GetMouseButtonDown(1))
        {
            WeaponManager.StartZooming();
        }

        // Weapon switching
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponManager.SwitchWeapon(WeaponType.Pistol);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponManager.SwitchWeapon(WeaponType.Sniper);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeaponManager.SwitchWeapon(WeaponType.Knife);
        }
    }

    // Called from Animation Event
    public void ShootEvent()
    {
        WeaponManager.OnShootAnimationEvent();
    }


    public void OnPickupHealth(int healthAdded)
    {
        health += healthAdded;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            Messenger<float, float>.Broadcast(GameEvent.HEALTH_CHANGED, health, maxHealth);
        }
    }

    public void Hit()
    {
        health -= 1;
        Messenger<float, float>.Broadcast(GameEvent.HEALTH_CHANGED, health, maxHealth);
        SoundManager.Instance.PlaySfx(SoundLibrary.Instance.sfxHit);
        if (health == 0)
        {
            if (health <= 0)
            {
                Messenger.Broadcast(GameEvent.PLAYER_DEAD);
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Messenger<int>.RemoveListener(GameEvent.PICKUP_HEALTH, OnPickupHealth);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        // does it have a rigidbody and is Physics enabled?
        if (body != null && !body.isKinematic)
        {
            body.linearVelocity = hit.moveDirection * pushForce;
        }
    }
}
