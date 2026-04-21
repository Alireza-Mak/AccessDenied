using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePopup : BasePopup
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button confirmButton;
    [SerializeField] private StartGamePopup startGamePopup;
    private UIManager uiManager;
    private void Start()
    {
        uiManager = GameObject.FindAnyObjectByType<UIManager>();
        // Disable confirm first
        confirmButton.interactable = false;
    }

    private void Update()
    {
        // Enable only with valid name
        confirmButton.interactable = !string.IsNullOrWhiteSpace(nameInput.text);
    }

    public void OnConfirm()
    {
        string enteredName = nameInput.text.Trim();

        if (string.IsNullOrWhiteSpace(enteredName))
            return;

        uiManager.UpdatePlayerName(enteredName);
        Close();
    }

    public void OnClose()
    {
        nameInput.text = "";
        Close();
        startGamePopup.Open();
    }
}
