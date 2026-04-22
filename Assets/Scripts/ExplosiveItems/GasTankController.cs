using UnityEngine;

public class GasTankController : BaseExplosive
{
    override protected void OnPlayerBulletHit(RaycastHit hit)
    {
        if (!hit.collider.gameObject.GetComponent<BaseExplosive>()) return;

        StartExplosion();
    }
}