using System.Collections;
using UnityEngine;

public class EnemyWeaponPickup : MonoBehaviour
{
    public BasicWeapon weaponPrefab;
    private bool isEnemyInRange = false;
    private AIWeaponController aiWeaponController;
    private static bool isPickingUpWeapon = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            aiWeaponController = collision.transform.root.GetComponent<AIWeaponController>();
            if (aiWeaponController != null && aiWeaponController.currentWeapon == null) // Добавлена проверка на отсутствие текущего оружия
            {
                isEnemyInRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            isEnemyInRange = false;
            aiWeaponController = null;
        }
    }

    private void Update()
    {
        if (isEnemyInRange && !isPickingUpWeapon && aiWeaponController.currentWeapon == null) // Добавлена проверка на отсутствие текущего оружия
        {
            StartCoroutine(PickupWeapon());
        }
    }

    private IEnumerator PickupWeapon()
    {
        isPickingUpWeapon = true; // Устанавливаем флаг перед началом подбора
        aiWeaponController.EquipWeapon(weaponPrefab);
        
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        // Отключаем коллайдер
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        yield return new WaitForSeconds(1f); // Ждём до конца кадра, чтобы убедиться, что все операции завершены
        isPickingUpWeapon = false; // Сбрасываем флаг после завершения подбора
        Destroy(gameObject);
    }
}