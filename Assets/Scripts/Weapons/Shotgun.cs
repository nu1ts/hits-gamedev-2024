using UnityEngine;

public class Shotgun : RangedWeapon
{
    public int pelletCount = 6;         // Количество снарядов, выпускаемых за один выстрел
    public float spreadPelletAngle = 15f;     // Угол разброса снарядов

    protected override void Shoot()
    {
        float stepAngle = spreadPelletAngle / (pelletCount - 1); // Угол между снарядами
        float startAngle = -spreadPelletAngle / 2;

        for (int i = 0; i < pelletCount; i++)
        {
            float angle = startAngle + i * stepAngle;
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, angle);
            Instantiate(projectilePrefab, firePoint.position, rotation);
        }

        // for (int i = 0; i < pelletCount; i++)
        // {
        //     // Вычисляем позицию для текущего объекта
        //     Vector3 offset = Quaternion.Euler(0, 0, spreadAngle) * Vector3.right * i;
        //     Vector3 newPosition = firePoint.position + offset;

        //     // Создаём объект
        //     GameObject obj = Instantiate(projectilePrefab, newPosition, Quaternion.identity);
        //     obj.transform.SetParent(transform); // Родитель для порядка
        // }

        if (muzzleFlashAnimator != null)
        {
            muzzleFlashAnimator.SetTrigger("Shoot");
        }

        playerWeaponController.CameraShake(cameraShakeDuration, cameraShakeMagnitude);

        ApplyRecoil();
    }
}
