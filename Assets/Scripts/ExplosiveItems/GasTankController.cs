using UnityEngine;

public class GasTankController : BaseExplosive
{
    protected override void OnPlayerBulletHit(RaycastHit hit)
    {
        BaseExplosive hitExplosive = hit.collider.GetComponentInParent<BaseExplosive>();
        if (hitExplosive != this) return;

        StartExplosion();
    }
}