using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePopup : BasePopup
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button confirmButton;
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

    public void OnConfirmButton()
    {
        string enteredName = nameInput.text.Trim();

        if (string.IsNullOrWhiteSpace(enteredName))
            return;

        uiManager.UpdatePlayerName(enteredName);
        nameInput.text = "";
        Close();
    }

    public override void OnCloseButton()
    {
        nameInput.text = "";
        base.OnCloseButton();

    }
}
