using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Other Settings")]
    [SerializeField] private TextMeshProUGUI ammo;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject zoomVignette;
    [SerializeField] private GameObject crosshair;

    [Header("Weapon Settings")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Sprite pistolSprite;
    [SerializeField] private Sprite sniperSprite;
    [SerializeField] private Sprite knifeSprite;



    private void Awake()
    {
        Messenger<float, float>.AddListener(GameEvent.AMMO_CHANGED, OnAmmoChanged);
        Messenger<WeaponType>.AddListener(GameEvent.WEAPON_CHANGED, OnGunChanged);
        Messenger<float, float>.AddListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
        Messenger<bool>.AddListener(GameEvent.ZOOM_CHANGED, OnZoomChanged);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        zoomVignette.SetActive(isZoom);
        crosshair.SetActive(!isZoom);

    }

    private void OnDestroy()
    {
        Messenger<float, float>.RemoveListener(GameEvent.AMMO_CHANGED, OnAmmoChanged);
        Messenger<WeaponType>.RemoveListener(GameEvent.WEAPON_CHANGED, OnGunChanged);
        Messenger<float, float>.RemoveListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
        Messenger<bool>.RemoveListener(GameEvent.ZOOM_CHANGED, OnZoomChanged);
    }

}
