using System.Collections;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public BasicWeapon currentWeapon;
    private bool isFiring;
    public Transform weaponPoint;
    
    public AmmoCounterUI ammoCounterUI;

    private void Update()
    {
        //Debug.Log(currentWeapon.IsWeaponOnCooldown());

        if (currentWeapon == null) return;

        if (Input.GetButtonDown("Fire1"))
        {
            if (currentWeapon.fireMode == FireMode.SemiAutomatic)
            {
                currentWeapon.UseWeapon();
            }
            else if (currentWeapon.fireMode == FireMode.Automatic)
            {
                isFiring = true;
                StartCoroutine(AutoFire());
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (currentWeapon.fireMode == FireMode.Automatic)
            {
                isFiring = false;
            }
        }
    }

    private IEnumerator AutoFire()
    {
        while (isFiring)
        {
            if (!currentWeapon.IsWeaponOnCooldown())
            {
                currentWeapon.UseWeapon();
            }
            yield return null;
        }
    }

    public void EquipWeapon(BasicWeapon newWeapon)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        currentWeapon = Instantiate(newWeapon, weaponPoint.position, weaponPoint.rotation, weaponPoint);
        currentWeapon.transform.SetParent(weaponPoint);

        if (currentWeapon is RangedWeapon)
        {
            ammoCounterUI.enabled = true;
            //(currentWeapon as RangedWeapon).UpdateAmmoUI();
        }
        else
        {
            ammoCounterUI.enabled = false;
        }
    }
}
