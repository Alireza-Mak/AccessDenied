using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammo;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject zoomVignette;



    private void Awake()
    {
        Messenger<float, float>.AddListener(GameEvent.AMMO_CHHANGED, OnAmmoChanged);
        Messenger<float, float>.AddListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
        Messenger<bool>.AddListener(GameEvent.ZOOM_CHANGED, OnZoomChanged);
    }

    private void OnAmmoChanged(float numOfAmmo, float maxAmmo)
    {
        ammo.text = "" + numOfAmmo + " / " + maxAmmo;
    }
    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
    private void OnZoomChanged(bool isZoom)
    {
        zoomVignette.SetActive(isZoom);
    }

    private void OnDestroy()
    {
        Messenger<float, float>.RemoveListener(GameEvent.AMMO_CHHANGED, OnAmmoChanged);
        Messenger<float, float>.RemoveListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
        Messenger<bool>.RemoveListener(GameEvent.ZOOM_CHANGED, OnZoomChanged);
    }

}
