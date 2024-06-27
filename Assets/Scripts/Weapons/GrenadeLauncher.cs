using UnityEngine;

public class GrenadeLauncher : RangedWeapon
{
    public GameObject grenadeShellPrefab;

    protected override void Shoot()
    {
        GameObject grenadeShellObject = Instantiate(grenadeShellPrefab, firePoint.position, firePoint.rotation);

        ApplyRecoil();

        if (muzzleFlashAnimator != null)
        {
            muzzleFlashAnimator.SetTrigger("Shoot");
        }
    }
}
