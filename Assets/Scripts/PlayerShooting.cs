using UnityEngine;

public class PlayerShooting : ShootingController
{
    protected override bool CanShoot()
    {
        return Input.GetButtonDown("Fire1");
    }
}