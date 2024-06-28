using System.Collections;
using State_Machine;
using UnityEngine;

public class AIWeaponController : MonoBehaviour
{
    [Header("Enemy Data")]
    public Enemy enemy;
    
    public BasicWeapon currentWeapon;
    private bool isFiring;
    public Transform weaponPoint;

    private bool _shouldFire;
    
    private void Update()
    {
        if (!currentWeapon) return;
        
        if (_shouldFire)
        {
            if (currentWeapon.fireMode == FireMode.SemiAutomatic)
            {
                currentWeapon.UseWeapon();
            }
            else if (currentWeapon.fireMode == FireMode.Automatic && !isFiring)
            {
                isFiring = true;
                StartCoroutine(AutoFire());
            }
        }
        else
        {
            isFiring = false;
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
        if (currentWeapon)
            Destroy(currentWeapon.gameObject);

        currentWeapon = Instantiate(newWeapon, weaponPoint.position, weaponPoint.rotation, weaponPoint);
        currentWeapon.transform.SetParent(weaponPoint);
    }

    public void SetFireState(bool state)
    {
        _shouldFire = state;
    }
}