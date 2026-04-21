using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Popup Window Settings")]
    [SerializeField] private GameOverPopup gameOverPopup;
    [SerializeField] private StartGamePopup startGamePopup;
    [SerializeField] private WinPopup winPopup;

    [Header("Weapon Settings")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Sprite pistolSprite;
    [SerializeField] private Sprite sniperSprite;
    [SerializeField] private Sprite knifeSprite;

    [Header("Other Settings")]
    [SerializeField] private TextMeshProUGUI ammo;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image accesskeyBar;
    [SerializeField] private GameObject zoomVignette;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private TextMeshProUGUI playerNameText;

    private int popupsActive = 0;
    private string playerName = "";


    private void Awake()
    {
        Messenger<float, float>.AddListener(GameEvent.AMMO_CHANGED, OnAmmoChanged);
        Messenger<WeaponType>.AddListener(GameEvent.WEAPON_CHANGED, OnGunChanged);
        Messenger<float, float>.AddListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
        Messenger<bool>.AddListener(GameEvent.ZOOM_CHANGED, OnZoomChanged);
        Messenger.AddListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.AddListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
    }

    private void Start()
    {
        //if (GameSession.HasStartedBefore && !string.IsNullOrWhiteSpace(GameSession.PlayerName))
        //{
        //    UpdatePlayerName(GameSession.PlayerName);
        //    return;
        //}
        //startGamePopup.Open();
        SoundManager.Instance.PlayMusic(SoundLibrary.Instance.music1);
    }
    public void UpdatePlayerName(string name)
    {
        playerName = name;
        playerNameText.text = name;
        GameSession.PlayerName = name;
        GameSession.HasStartedBefore = true;
    }


    private void OnAmmoChanged(float numOfAmmo, float maxAmmo)
    {
        ammo.text = "" + numOfAmmo + " / " + maxAmmo;
    }
    private void OnGunChanged(WeaponType weaponType)
    {
        ammo.gameObject.transform.parent.gameObject.SetActive(true);
        switch (weaponType)
        {
            case WeaponType.Pistol:
                weaponIcon.sprite = pistolSprite;
                break;
            case WeaponType.Sniper:
                weaponIcon.sprite = sniperSprite;
                break;
            case WeaponType.Knife:
                weaponIcon.sprite = knifeSprite;
                ammo.gameObject.transform.parent.gameObject.SetActive(false);
                break;
        }
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    private void OnZoomChanged(bool isZoom)
    {
        SoundManager.Instance.PlaySfx(SoundLibrary.Instance.sfxZoom);
        zoomVignette.SetActive(isZoom);
        crosshair.SetActive(!isZoom);
    }

    public void UpdateKeyCard(float keyCards)
    {
        accesskeyBar.fillAmount = keyCards;
    }

    public void ShowGameOverPopup(float timer, int keys, int score)
    {
        gameOverPopup.Open();
        gameOverPopup.UpdateRecords(TimeConvertor(timer), keys.ToString(), score.ToString());
    }
    public void ShowWinPopup(float timer, int keys, int score)
    {
        winPopup.Open();
        winPopup.UpdateRecords(TimeConvertor(timer), keys.ToString(), score.ToString());
    }
    public void OnPopupOpened()
    {
        if (popupsActive == 0)
        {
            SetGameActive(false);
        }
        popupsActive++;
    }
    public void OnPopupClosed()
    {
        popupsActive--;
        if (popupsActive == 0)
        {
            SetGameActive(true);
        }
    }
    public void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

    public void SetGameActive(bool active)
    {
        if (active)
        {
            Messenger.Broadcast(GameEvent.GAME_ACTIVE);
            Time.timeScale = 1; // unpause the game
            Cursor.lockState = CursorLockMode.Locked; // lock cursor at center
            Cursor.visible = false; // hide cursor
            crosshair.SetActive(true); // show the crosshair
        }
        else
        {
            Messenger.Broadcast(GameEvent.GAME_INACTIVE);
            Time.timeScale = 0; // pause the game
            Cursor.lockState = CursorLockMode.None; // let cursor move freely
            Cursor.visible = true; // show the cursor
            crosshair.SetActive(false); // turn off the crosshair
        }
    }

    public void UpdateTimerUI(float timeElapsed)
    {
        timerText.text = TimeConvertor(timeElapsed);
    }

    private string TimeConvertor(float timeElapsed)
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnDestroy()
    {
        Messenger<float, float>.RemoveListener(GameEvent.AMMO_CHANGED, OnAmmoChanged);
        Messenger<WeaponType>.RemoveListener(GameEvent.WEAPON_CHANGED, OnGunChanged);
        Messenger<float, float>.RemoveListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
        Messenger<bool>.RemoveListener(GameEvent.ZOOM_CHANGED, OnZoomChanged);
        Messenger.RemoveListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.RemoveListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
    }

}
