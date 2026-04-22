using UnityEngine;

public class MainMenuPopup : BasePopup
{
    public void OnResumeButton()
    {
        Close();
    }
    public void OnSetingsButton()
    {
        Close();
        SettingsPopup settingsPopup = UnityEngine.GameObject.FindAnyObjectByType<SettingsPopup>(UnityEngine.FindObjectsInactive.Include);
        settingsPopup.Open();
        settingsPopup.SetPreviousPopup(this);
    }
}
