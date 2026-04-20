using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponController[] weapons;
    [SerializeField] Animator Animator;
    [SerializeField] private int currentWeaponIndex = 0;

    private WeaponController currentWeapon;

    void Start()
    {
        if (weapons == null || weapons.Length == 0)
            return;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].gameObject.SetActive(false);
            }
        }

        EquipWeapon(currentWeaponIndex);
        Messenger<WeaponType>.Broadcast(GameEvent.WEAPON_CHANGED, currentWeapon.GetWeaponType());
    }

    public void StartFiring()
    {
        if (currentWeapon.CurrentNumbOfAmmo <= 0 && currentWeapon.GetWeaponType() != WeaponType.Knife)
        {
            return;
        }
        Animator.SetTrigger("shoot");
        currentWeapon.OnPrimaryActionDown();
    }

    public void StartZooming()
    {
        if (currentWeapon.GetWeaponType() == WeaponType.Sniper)
        {
            currentWeapon.OnSecondaryActionDown();
        }
    }

    public void SwitchWeapon(WeaponType weaponType)
    {
        EquipWeaponByType(weaponType);
        Messenger<WeaponType>.Broadcast(GameEvent.WEAPON_CHANGED, weaponType);
    }

    public void EquipWeapon(int index)
    {
        if (weapons == null || weapons.Length == 0)
            return;

        if (index < 0 || index >= weapons.Length)
            return;

        if (currentWeapon != null)
        {
            currentWeapon.Unequip();
        }

        currentWeaponIndex = index;
        currentWeapon = weapons[currentWeaponIndex];

        if (currentWeapon != null)
        {
            currentWeapon.Equip();
        }

        currentWeapon.OnWeaponChanged();
    }

    public void EquipWeaponByType(WeaponType weaponType)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null && weapons[i].GetWeaponType() == weaponType)
            {
                EquipWeapon(i);
                return;
            }
        }
    }


    public void OnShootAnimationEvent()
    {
        currentWeapon.PlayMuzzleFlash();
    }

    public WeaponController GetCurrentWeapon()
    {
        return currentWeapon;
    }
}