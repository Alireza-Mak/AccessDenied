using UnityEngine;

public class SniperController : WeaponController
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomFieldOfView = 15f;
    [SerializeField] private float normalFieldOfView = 60f;

    private bool isZoomed;

    public override void OnSecondaryActionDown()
    {
        if (!isEquipped || playerCamera == null)
            return;

        isZoomed = !isZoomed;
        playerCamera.fieldOfView = isZoomed ? zoomFieldOfView : normalFieldOfView;

        Messenger<bool>.Broadcast(GameEvent.ZOOM_CHANGED, isZoomed);
    }

    public override void Unequip()
    {
        ResetZoom();
        base.Unequip();
    }

    private void ResetZoom()
    {
        isZoomed = false;

        if (playerCamera != null)
        {
            playerCamera.fieldOfView = normalFieldOfView;
        }

        Messenger<bool>.Broadcast(GameEvent.ZOOM_CHANGED, false);
    }
}