using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    public float explosionDelay = 3f;
    public int damage = 50;
    public LayerMask enemyLayers;
    public GameObject explosionPrefab;

    private Collider2D explosionCollider;
    private bool hasExploded = false;

    private Rigidbody2D rb;

    public float cameraShakeDuration = 0.1f;
    public float cameraShakeMagnitude = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;


        explosionCollider = GetComponent<Collider2D>();
        explosionCollider.enabled = false; // Отключаем коллайдер по умолчанию

        StartCoroutine(ExplodeAfterDelay());
        StartCoroutine(SlowDownOverTime());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    private void Explode()
    {
        if (hasExploded) return;

        hasExploded = true;

        
        // Включаем коллайдер для обнаружения врагов
        StartCoroutine(EnableColliderAndDamage());
    }

    private IEnumerator EnableColliderAndDamage()
    {
        yield return new WaitForSeconds(1f); // Задержка перед включением коллайдера, если требуется

        explosionCollider.enabled = true;

        // Получаем всех врагов в зоне действия
        Collider2D[] hitObjects = Physics2D.OverlapBoxAll(explosionCollider.bounds.center, explosionCollider.bounds.size, 0f, enemyLayers);

        foreach (Collider2D hit in hitObjects)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage, 0f);
            }
        }

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        }

        GlobalEvents.TriggerCameraShake(cameraShakeDuration, cameraShakeMagnitude);

        Destroy(gameObject);
    }

    private IEnumerator SlowDownOverTime()
    {
        float timeElapsed = 0f;
        while (!hasExploded)
        {
            speed *= 0.3f; // Коэффициент замедления, можно настроить по необходимости
            timeElapsed += Time.deltaTime;
            yield return new WaitForSeconds(0.1f); // Частота обновления замедления, можно настроить по необходимости
        }
    }
}
