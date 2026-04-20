using UnityEngine;

public class ActiveDuringGameplay : MonoBehaviour

{
    void Awake()
    {
        Messenger.AddListener(GameEvent.GAME_ACTIVE, OnGameActive);
        Messenger.AddListener(GameEvent.GAME_INACTIVE, OnGameInctive);
    }
    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GAME_ACTIVE, OnGameActive);
        Messenger.RemoveListener(GameEvent.GAME_INACTIVE, OnGameInctive);
    }

    private void OnGameActive()
    {
        this.enabled = true;
    }
    private void OnGameInctive()
    {
        this.enabled = false;
    }
}