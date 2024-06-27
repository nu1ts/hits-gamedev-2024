using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 20; // Количество здоровья, которое восстанавливает аптечка
    public GameObject healParticlePrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthController healthController = collision.GetComponent<HealthController>();
        if (healthController != null)
        {
            healthController.Heal(healAmount);

            // Воспроизводим систему частиц лечения
            if (healParticlePrefab != null)
            {
                GameObject healEffect = Instantiate(healParticlePrefab, collision.transform.position, Quaternion.identity);
                healEffect.transform.SetParent(collision.transform); // Устанавливаем эффект как дочерний объект героя

                // Уничтожаем систему частиц после её завершения
                ParticleSystem particleSystem = healEffect.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    Destroy(healEffect, particleSystem.main.duration);
                }
            }

            Destroy(gameObject); // Уничтожаем аптечку после использования
        }
    }
}
