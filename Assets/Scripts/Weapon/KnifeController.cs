//using UnityEngine;

//public class KnifeController : WeaponController
//{
//    [Header("Knife Settings")]
//    [SerializeField] private float attackRange = 2f;

//    public override void OnPrimaryActionDown()
//    {
//        if (!isEquipped)
//            return;

//        Attack();
//    }

//    private void Attack()
//    {
//        if (weaponAnimator != null)
//        {
//            weaponAnimator.SetTrigger("shoot");
//        }

//        if (playerCamera == null)
//            return;

//        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

//        if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
//        {
//            if (hit.collider.CompareTag("Enemy"))
//            {
//                Debug.Log("Knife hit enemy");
//                // Add knife damage logic here later.
//            }
//        }
//    }
//}