using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public enum ItemType
    {
        Health,
        Ammo,
        KeyCard,
        Floppy
    }

    [Header("Item Settings")]
    [SerializeField] protected ItemType itemType;
    [SerializeField] protected int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        switch (itemType)
        {
            case ItemType.Health:
                Messenger<int>.Broadcast(GameEvent.PICKUP_HEALTH, value);
                break;

            case ItemType.Ammo:
                Messenger<int>.Broadcast(GameEvent.PICKUP_AMMO, value);
                break;

            case ItemType.KeyCard:
                Messenger<int>.Broadcast(GameEvent.PICKUP_KEYCARD, value);
                break;
            case ItemType.Floppy:
                Messenger.Broadcast(GameEvent.PICKUP_FLOPPY);
                break;
        }

        Destroy(gameObject);
    }
}