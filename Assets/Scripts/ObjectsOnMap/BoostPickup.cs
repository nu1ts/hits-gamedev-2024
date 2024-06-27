using System.Collections;
using UnityEngine;

public class BoostPickup : MonoBehaviour
{
    public GameObject boostParticlePrefab; // Префаб системы частиц лечения
    public float timeSlowDuration = 5f; // Длительность замедления времени
    public float timeSlowScale = 0.5f; // Масштаб замедления времени

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthController healthController = collision.GetComponent<HealthController>();
        if (healthController != null)
        {
            if (boostParticlePrefab != null)
            {
                GameObject boostEffect = Instantiate(boostParticlePrefab, collision.transform.position, Quaternion.identity);
                boostEffect.transform.SetParent(collision.transform); // Устанавливаем эффект как дочерний объект героя

                // Уничтожаем систему частиц после её завершения
                ParticleSystem particleSystem = boostEffect.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    Destroy(boostEffect, particleSystem.main.duration);
                }
                else
                {
                    Destroy(boostEffect, 2f); // Уничтожаем через 2 секунды, если система частиц не найдена
                }
            }

            // Активируем замедление времени
            PlayerMovement playerMovement = collision.GetComponentInParent<PlayerMovement>();
            if (playerMovement != null)
            {
                TimeManager.instance.SlowTime(timeSlowDuration, timeSlowScale, playerMovement);
            }

            Destroy(gameObject); // Уничтожаем аптечку после использования
        }
    }
}
