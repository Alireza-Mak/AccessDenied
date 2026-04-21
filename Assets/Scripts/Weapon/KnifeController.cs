using UnityEngine;
using UnityEngine.Timeline;

public class KnifeController : WeaponController
{
    [Header("Knife Settings")]
    [SerializeField] private float attackRange = 1.5f;

    public override void OnPrimaryActionDown()
    {
        if (!isEquipped)
            return;
        Cooldown();
        MeleeAttack();
    }

    private void MeleeAttack()
    {
        Collider[] hits = Physics.OverlapSphere(FireSpawnPoint.position, attackRange);

        foreach (Collider hit in hits)
        {
            Enemy enemy = hit.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
                break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(FireSpawnPoint.position, attackRange);
    }
}