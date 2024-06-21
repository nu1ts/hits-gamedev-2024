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

    public abstract void UseWeapon();

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
}
