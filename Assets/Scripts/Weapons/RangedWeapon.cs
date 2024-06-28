using System.Collections;
using UnityEngine;

public class RangedWeapon : BasicWeapon
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Transform muzzleFlashPoint;

    public int maxAmmo = 30;            // Максимальное количество патронов в магазине
    public float reloadTime = 1.5f;     // Время перезарядки
    public float recoilForce = 10f;     // Сила отдачи
    public float recoilDuration = 0.1f; // Длительность отдачи

    private int currentAmmo;            // Текущее количество патронов в магазине
    private bool isReloading;

    public float cameraShakeDuration = 0.1f; // Длительность тряски камеры
    public float cameraShakeMagnitude = 0.2f;

    private Rigidbody2D playerRb;
    public AmmoCounterUI ammoCounterUI;

    public GameObject muzzleFlashPrefab;
    protected Animator muzzleFlashAnimator;

    public float spreadAngle = 5f;
    public bool canRicochet = false;

    private void Start()
    {
        if (muzzleFlashPrefab != null && muzzleFlashPoint != null)
        {
            GameObject muzzleFlashInstance = Instantiate(muzzleFlashPrefab, muzzleFlashPoint.position, muzzleFlashPoint.rotation, muzzleFlashPoint);
            muzzleFlashAnimator = muzzleFlashInstance.GetComponent<Animator>();
        }
    }

    private void Awake()
    {
        currentAmmo = maxAmmo;
        playerRb = GetComponentInParent<Rigidbody2D>();
        UpdateAmmoUI();
    }

    public override void UseWeapon()
    {
        if (isReloading || isCooldown)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        Shoot();
        currentAmmo--;
        UpdateAmmoUI();
        StartCoroutine(Cooldown());
    }

    public void UpdateAmmoUI()
    {
        if (ammoCounterUI != null)
        {
            ammoCounterUI.UpdateAmmoCount(currentAmmo, maxAmmo);
        }
    }

    protected virtual void Shoot()
    {
        // Генерируем случайный угол в диапазоне [-spreadAngle/2, spreadAngle/2]
        float randomAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2);

        // Поворачиваем firePoint на случайный угол
        Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, randomAngle);

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, rotation);
        Bullet bullet = projectile.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.canRicochet = canRicochet;
            bullet.damage = damage;
        }

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        ApplyRecoil();

        playerWeaponController.CameraShake(cameraShakeDuration, cameraShakeMagnitude);

        if (muzzleFlashAnimator != null)
        {
            muzzleFlashAnimator.SetTrigger("Shoot");
        }
    }

    //Первая реализация Отдачи (как в HealthController)
    protected IEnumerator ApplyRecoil1()
    {
        Vector2 recoilDirection = -firePoint.up * recoilForce;

        float elapsedTime = 0;
        while (elapsedTime < recoilDuration)
        {
            playerRb.MovePosition(playerRb.position + recoilDirection * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    //Вторая реализация Отдачи
    protected void ApplyRecoil()
    {
        Vector2 recoilDirection = -firePoint.up * recoilForce;

        if (playerRb != null)
        {
            // Применяем силу отдачи
            playerRb.velocity = recoilDirection;
            StartCoroutine(StopRecoil());
        }
    }

    private IEnumerator StopRecoil()
    {
        // Ждем немного времени
        yield return new WaitForSeconds(0.1f);

        // Останавливаем движение игрока
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
        }
    }

    private IEnumerator Reload()
    {
        if (isReloading)
            yield break;

        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        isReloading = false;
    }
}
