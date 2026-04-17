using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    // Using this method in events section of Shoot Animation
    public void OnShootFrame()
    {
        Messenger.Broadcast(GameEvent.SHOOT_FRAME);
    }
}
