using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponController[] weapons;
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
    }

    void Update()
    {
        if (currentWeapon == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            currentWeapon.OnPrimaryActionDown();
        }

        if (Input.GetMouseButtonDown(1) && currentWeapon.GetWeaponType() == WeaponType.Sniper)
        {
            currentWeapon.OnSecondaryActionDown();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeaponByType(WeaponType.Pistol);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeaponByType(WeaponType.Sniper);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeaponByType(WeaponType.Knife);
        }
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

    public WeaponController GetCurrentWeapon()
    {
        return currentWeapon;
    }
}