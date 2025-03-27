using System.Collections;
using UnityEngine;

public enum FireMode
{
    SemiAutomatic,
    Automatic
}

public abstract class BasicWeapon : MonoBehaviour
{
    public string weaponName;
    public float cooldownTime;
    public int damage;
    protected bool isCooldown;
    public FireMode fireMode = FireMode.Automatic;
    public int cost;

    protected PlayerWeaponController playerWeaponController;
    protected AIWeaponController aiWeaponController;

    public WeaponPickup weaponPickup;

    public abstract void UseWeapon();

    public void SetPlayerWeaponController(PlayerWeaponController controller)
    {
        playerWeaponController = controller;
    }

    public void SetAiWeaponController(AIWeaponController controller)
    {
        aiWeaponController = controller;
    }

    protected IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }

    public bool IsWeaponOnCooldown()
    {
        return isCooldown;
    }

    public void DropWeapon()
    {
        Instantiate(weaponPickup, transform.position, transform.rotation);
    }
}
