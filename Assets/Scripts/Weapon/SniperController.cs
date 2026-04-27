using UnityEngine;

public class SniperController : WeaponController
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomFieldOfView = 15f;
    [SerializeField] private float normalFieldOfView = 60f;

    public bool IsZoomed{ get; private set; }

    public override void OnSecondaryActionDown()
    {
        if (!isEquipped || playerCamera == null)
            return;

        IsZoomed = !IsZoomed;
        playerCamera.fieldOfView = IsZoomed ? zoomFieldOfView : normalFieldOfView;

        Messenger<bool>.Broadcast(GameEvent.ZOOM_CHANGED, IsZoomed);
    }

    public override void Unequip()
    {
        if (IsZoomed)
        {
            ResetZoom();

        }
        base.Unequip();
    }

    private void ResetZoom()
    {
        IsZoomed = false;

        if (playerCamera != null)
        {
            playerCamera.fieldOfView = normalFieldOfView;
        }

        Messenger<bool>.Broadcast(GameEvent.ZOOM_CHANGED, false);
    }
}