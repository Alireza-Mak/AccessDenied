using TMPro;
using UnityEditor;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    private BasePopup previousPopup;
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

    virtual public void OnHomeButton()
    {
        GameSession.ResetSession();
        Messenger.Broadcast(GameEvent.RESTART_GAME);
    }

    virtual public void UpdateRecords(string timer, string keys, string score)
    {
        Transform recordsContainer = transform.Find("RecordsContainer");
        recordsContainer.Find("TimerValue").GetComponent<TextMeshProUGUI>().text = timer;
        recordsContainer.Find("KeysValue").GetComponent<TextMeshProUGUI>().text = keys;
        recordsContainer.Find("ScoreValue").GetComponent<TextMeshProUGUI>().text = score;
    }

    virtual public void OnInfoButton()
    {
        Close();
        InfoPopup infoPopup = GameObject.FindAnyObjectByType<InfoPopup>(FindObjectsInactive.Include);
        infoPopup.Open();
        infoPopup.SetPreviousPopup(this);
    }

    public bool IsActive()
    {
        return this != null && gameObject != null && gameObject.activeSelf;
    }



    virtual public void OnCloseButton()
    {
        Close();

        if (previousPopup == null) return;

        previousPopup.Open();
    }

    public void SetPreviousPopup(BasePopup prevPopup)
    {
        previousPopup = prevPopup;
    }
}
