using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponController : MonoBehaviour
{
    public BasicWeapon currentWeapon;
    private bool isFiring;
    public Transform weaponPoint;
    
    public AmmoCounterUI ammoCounterUI;

    private CameraController _cameraController;

    public BasicWeapon initialWeaponPrefab;

    private void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();

        InitializeInitialWeapon();
    }

     private void OnEnable()
    {
        GlobalEvents.OnExplosion += CameraShake;
    }

    private void OnDisable()
    {
        GlobalEvents.OnExplosion -= CameraShake;
    }

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
                // if (_cameraController != null)
                // {
                //     TriggerCameraShake();
                // }
            }
            yield return null;
        }
    }

    public void EquipWeapon(BasicWeapon newWeapon)
    {
        if (currentWeapon != null)
        {
            if(!(currentWeapon is MeleeWeapon))
            {
                currentWeapon.DropWeapon();
            }
            Destroy(currentWeapon.gameObject);
        }

        currentWeapon = Instantiate(newWeapon, weaponPoint.position, weaponPoint.rotation, weaponPoint);
        currentWeapon.transform.SetParent(weaponPoint);

        if (currentWeapon is RangedWeapon)
        {
            ammoCounterUI.enabled = true;
            RangedWeapon rangedWeapon = currentWeapon as RangedWeapon;
            if (rangedWeapon != null)
            {
                rangedWeapon.ammoCounterUI = this.ammoCounterUI;
                rangedWeapon.UpdateAmmoUI();
            }
        }
        else
        {
            ammoCounterUI.enabled = false;
        }

        currentWeapon.SetPlayerWeaponController(this);
    }

    public void CameraShake(float cameraShakeDuration, float cameraShakeMagnitude)
    {
        _cameraController.ShakeCamera(cameraShakeDuration, cameraShakeMagnitude);
    }

    private void InitializeInitialWeapon()
    {
        if (initialWeaponPrefab != null)
        {
            EquipWeapon(initialWeaponPrefab);
        }
    }
}
