using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePopup : BasePopup
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject errorText;
    private UIManager uiManager;
    private void Start()
    {
        uiManager = GameObject.FindAnyObjectByType<UIManager>();
        errorText.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) ||
            Input.GetKeyDown(KeyCode.Return)
            )
        {
            OnConfirmButton();
        }
        if (!string.IsNullOrWhiteSpace(nameInput.text))
        {
            errorText.SetActive(false);
        }
    }

    public void OnConfirmButton()
    {
        string enteredName = nameInput.text.Trim();

        if (string.IsNullOrWhiteSpace(enteredName))
        {
            errorText.SetActive(true);
            return;
        }
        errorText.SetActive(false);
        uiManager.UpdatePlayerName(enteredName);
        nameInput.text = "";
        Close();
    }

    public override void OnCloseButton()
    {
        errorText.SetActive(false);
        nameInput.text = "";
        base.OnCloseButton();
    }
}
