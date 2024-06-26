using UnityEngine;

public class CrystalMagnet : MonoBehaviour
{
    public float attractionForce = 1f; // Сила притяжения кристалла к игроку

    public float attractionDistance = 3f; 
    private Transform playerTransform; // Ссылка на трансформ игрока
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        //rb.isKinematic = true; // Делаем объект кинематическим для управления вручную
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            AttractToPlayer();
        }
    }

    private void AttractToPlayer()
    {
        Vector2 direction = playerTransform.position - transform.position;
        float distance = direction.magnitude;

        if (distance < attractionDistance)
        {
            float forceMagnitude = attractionForce * (1 - distance / attractionDistance);
            Vector2 force = direction.normalized * forceMagnitude;
            rb.velocity = force;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ENTERED CRYSTAL MAGNET");
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
