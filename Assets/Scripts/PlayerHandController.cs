using Unity.VisualScripting;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    public Transform equippedWeaponParent;  // Родитель для точки крепления оружия (Equipped Weapon)
    public Transform rightArm;  // Ссылка на правую руку
    public GameObject initialWeaponPrefab;  // Префаб начального оружия

    private GameObject currentWeaponInstance;  // Текущий экземпляр оружия
    private Transform currentWeaponPoint;  // Точка, где должно быть текущее оружие

    private void Start()
    {
        // Если есть начальное оружие, применяем его
        if (initialWeaponPrefab != null)
        {
            EquipWeapon(initialWeaponPrefab);
        }
    }

    // Метод для установки оружия
    public void EquipWeapon(GameObject weaponPrefab)
    {
        // Открепляем предыдущее оружие, если оно есть
        UnequipWeapon();

        // Создаем новое оружие и крепим его к точке крепления
        currentWeaponInstance = Instantiate(weaponPrefab);
        string weaponName = weaponPrefab.name.Replace("Equipped", "").Trim();
        currentWeaponPoint = equippedWeaponParent.Find(weaponName + "Point");

        if (currentWeaponPoint != null)
        {
            currentWeaponInstance.transform.SetParent(currentWeaponPoint, false);
            currentWeaponInstance.transform.localPosition = Vector3.zero;
            currentWeaponInstance.transform.localRotation = Quaternion.identity;

            // Позиционируем правую руку на точке Right Hand Point оружия
            Transform rightHandPointInWeapon = currentWeaponInstance.transform.Find("RightHandPoint");

            Debug.Log(rightHandPointInWeapon.IsUnityNull());

            if (rightHandPointInWeapon != null)
            {
                rightArm.position = rightHandPointInWeapon.position;
                rightArm.rotation = rightHandPointInWeapon.rotation;
            }
        }
        else
        {
            Debug.LogWarning("Weapon point not found for: " + weaponName);
        }
    }

    // Метод для снятия оружия
    public void UnequipWeapon()
    {
        // Уничтожаем текущее оружие, если оно есть
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
        }
        currentWeaponPoint = null;
    }
}
