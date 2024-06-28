using System.Collections;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public BasicWeapon weaponPrefab;
    private bool isPlayerInRange = false;
    private PlayerWeaponController playerWeaponController;
    private static bool isPickingUpWeapon = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerWeaponController = collision.transform.root.GetComponent<PlayerWeaponController>();
            if (playerWeaponController != null)
            {
                isPlayerInRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerWeaponController = null;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && !isPickingUpWeapon)
        {
            if (playerWeaponController != null)
            {
                //playerWeaponController.EquipWeapon(weaponPrefab);
                //Destroy(gameObject); // Уничтожаем объект оружия на сцене после подбора
                StartCoroutine(PickupWeapon());
            }
        }
    }

    private IEnumerator PickupWeapon()
    {
        isPickingUpWeapon = true; // Устанавливаем флаг перед началом подбора
        playerWeaponController.EquipWeapon(weaponPrefab);

        WeaponManager.instance.EquipWeapon(weaponPrefab);
        
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
