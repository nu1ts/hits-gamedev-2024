using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Teleport linkedTeleport; // Ссылка на другой телепортирующий блок

    private bool isTeleporting = false; // Флаг для предотвращения зацикливания

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Bullet")) && linkedTeleport != null && !isTeleporting)
        {
            StartCoroutine(TeleportObject(other.transform.root));
        }
    }

    private IEnumerator TeleportObject(Transform obj)
    {
        isTeleporting = true;

        // Отключаем телепортацию на короткое время, чтобы предотвратить повторный телепорт
        linkedTeleport.isTeleporting = true;

        // Телепортируем объект к связанному телепорту
        obj.position = linkedTeleport.transform.position;

        yield return new WaitForSeconds(0.5f); // Время ожидания перед повторной телепортацией

        isTeleporting = false;
        linkedTeleport.isTeleporting = false;
    }
}
