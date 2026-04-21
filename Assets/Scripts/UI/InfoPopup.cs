public class InfoPopup : BasePopup
{
    private BasePopup previousPopup;
    public void OncloseButton()
    {
        Close();
        previousPopup.Open();

    }
    public void SetPreviousPopup(BasePopup prevPopup)
    {
        previousPopup = prevPopup;
    }

}
