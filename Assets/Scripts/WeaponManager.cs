using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance; // Singleton instance

    public CameraController cameraController; // ссылка на CameraController

    public float normalCameraDistance = 10f; // нормальное значение distance
    public float sniperCameraDistance = 20f; // значение distance для снайперской винтовки

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EquipWeapon(BasicWeapon newWeapon)
    {
        if (newWeapon.weaponName == "SniperRifle")
        {
            cameraController.SetDistance(sniperCameraDistance);
        }
        else
        {
            cameraController.SetDistance(normalCameraDistance);
        }
    }
}
