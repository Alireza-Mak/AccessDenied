using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private int health;
    public static readonly int maxHealth = 5;

    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.PICKUP_HEALTH, OnPickupHealth);
        health = maxHealth;
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
