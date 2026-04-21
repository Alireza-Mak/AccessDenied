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
                SoundManager.Instance.PlaySfx(SoundLibrary.Instance.sfxCollectHealth);
                Messenger<int>.Broadcast(GameEvent.PICKUP_HEALTH, value);
                break;

            case ItemType.Ammo:
                Messenger<int>.Broadcast(GameEvent.PICKUP_AMMO, value);
                SoundManager.Instance.PlaySfx(SoundLibrary.Instance.sfxCollectAmmo);
                break;

            case ItemType.KeyCard:
                SoundManager.Instance.PlaySfx(SoundLibrary.Instance.sfxCollectKey);
                Messenger<int>.Broadcast(GameEvent.PICKUP_KEYCARD, value);
                break;
            case ItemType.Floppy:
                SoundManager.Instance.PlaySfx(SoundLibrary.Instance.sfxCollectFloppy);
                Messenger.Broadcast(GameEvent.PICKUP_FLOPPY);
                break;
        }

        Destroy(gameObject);
    }
}