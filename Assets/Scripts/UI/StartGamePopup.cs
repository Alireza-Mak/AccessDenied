using UnityEngine;

public class StartGamePopup : BasePopup
{
    public void OnStartButton()
    {
        Close();
        PlayerNamePopup playerNamePopup = UnityEngine.GameObject.FindAnyObjectByType<PlayerNamePopup>(UnityEngine.FindObjectsInactive.Include);
        playerNamePopup.Open();
        playerNamePopup.SetPreviousPopup(this);
    }
    public void OnSetingsButton()
    {
        Close();
        SettingsPopup settingsPopup = UnityEngine.GameObject.FindAnyObjectByType<SettingsPopup>(UnityEngine.FindObjectsInactive.Include);
        settingsPopup.Open();
        settingsPopup.SetPreviousPopup(this);
    }
}
