using UnityEngine;

public class EnemyWeaponManager : MonoBehaviour
{
    public Transform PistolFireSpawnPoint;
    public void PlayMuzzleFlash()
    {
        PistolFireSpawnPoint.GetChild(0).GetComponent<ParticleSystem>().Play();
    }
}
