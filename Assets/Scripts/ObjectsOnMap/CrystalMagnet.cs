using Unity.Mathematics;
using UnityEngine;

public class CrystalMagnet : MonoBehaviour
{
    public float attractionForce = 1f; // Сила притяжения кристалла к игроку

    public float attractionDistance = 3f; 
    private Transform playerTransform; // Ссылка на трансформ игрока
    private Rigidbody2D rb;
    private Quaternion initialRotation; // Исходная ротация

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        rb.isKinematic = true; // Делаем объект кинематическим для управления вручную
        initialRotation = rb.transform.rotation;
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            AttractToPlayer();
        }
        else
        {
            ReturnToInitialRotation();
        }
    }

    private void AttractToPlayer()
    {
        Vector2 direction = playerTransform.position - transform.position;
        float distance = direction.magnitude;

        if (distance < attractionDistance)
        {
            // Притягиваем к игроку
            float forceMagnitude = attractionForce * (1 - distance / attractionDistance);
            Vector2 force = direction.normalized * forceMagnitude;
            rb.velocity = force;

            // Поворачиваем к игроку
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void ReturnToInitialRotation()
    {
        // Возвращаем в исходное положение
        rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, initialRotation, 200 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
            rb.velocity = Vector2.zero;
        }
    }
}
