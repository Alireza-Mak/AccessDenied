using UnityEditor;
using UnityEngine;

public class BasePopup : MonoBehaviour
{

    virtual public void Open()
    {
        if (this == null || gameObject == null) return;

        if (!IsActive())
        {
            this.gameObject.SetActive(true);
            Messenger.Broadcast(GameEvent.POPUP_OPENED);
        }
        else
        {
            Debug.LogError(this + ".Open() – trying to open a popup that is active!");
        }
    }

    virtual public void Close()
    {
        if (IsActive())
        {
            if (this == null || gameObject == null) return;

            this.gameObject.SetActive(false);
            Messenger.Broadcast(GameEvent.POPUP_CLOSED);
        }
        else
        {
            Debug.LogError(this + ".Close() – trying to close a popup that is not active!");
        }
    }


    virtual public void OnQuitButton()
    {
        GameSession.ResetSession();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
    virtual public void OnRetryButton()
    {
        Close();
        Messenger.Broadcast(GameEvent.RESTART_GAME);
    }

    public bool IsActive()
    {
        return this != null && gameObject != null && gameObject.activeSelf;
    }
}
