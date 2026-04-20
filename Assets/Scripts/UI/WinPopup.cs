using TMPro;

public class WinPopup : BasePopup
{
    public void UpdateTimer(string timer)
    {
        transform.Find("TimerValue").GetComponent<TextMeshProUGUI>().text = timer;
    }
    public void UpdateAccessKeys(string keys)
    {
        transform.Find("KeysValue").GetComponent<TextMeshProUGUI>().text = keys;
    }
}
