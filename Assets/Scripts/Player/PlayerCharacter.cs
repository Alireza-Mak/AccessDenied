using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private int health;
    public static readonly int maxHealth = 5;
    [SerializeField] private WeaponManager WeaponManager;
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.PICKUP_HEALTH, OnPickupHealth);
        health = maxHealth;
        Animator = GetComponent<Animator>();

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
            Animator.SetTrigger("shoot");
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
        if (health == 0)
        {
            if (health <= 0)
            {
                Messenger.Broadcast(GameEvent.PLAYER_DEAD);
            }
        }
    }

    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.PICKUP_HEALTH, OnPickupHealth);
    }
}
