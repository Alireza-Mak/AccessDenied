using TMPro;
using UnityEngine;

public class StartGamePopup : BasePopup
{
    [SerializeField] private PlayerNamePopup playerNamePopup;
    public void OnStartButton()
    {
        Close();
        playerNamePopup.Open();
    }
}
