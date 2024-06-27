using UnityEngine;

public class GrenadeShell : MonoBehaviour
{
    public GameObject explosionPrefab; // Префаб взрыва

    public float speed = 25f; // Задержка перед взрывом
    public float explosionDelay = 3f; // Задержка перед взрывом
    public float explosionRadius = 5f; // Радиус взрыва
    public int explosionDamage = 50; // Урон от взрыва
    public float knockbackForce = 20f;

    private bool hasExploded = false;
    private Rigidbody2D rb;
    public LayerMask collisionLayers;
    private Vector2 previousPosition;
    private bool hasExplode = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
        previousPosition = rb.position;
    }

    private void Start()
    {
        Invoke("Explode", explosionDelay);
        Destroy(gameObject, explosionDelay);
    }

    private void FixedUpdate()
    {
        // Проверка пересечения пули с объектами
        RaycastHit2D hit = Physics2D.Raycast(previousPosition, transform.up, speed * Time.fixedDeltaTime, collisionLayers);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Vector2 reflectedDirection = Vector2.Reflect(rb.velocity.normalized, hit.normal);

                // Применяем отраженное направление пули
                rb.velocity = reflectedDirection * speed;

                // Разворачиваем пулю в сторону отражения
                float angle = Mathf.Atan2(reflectedDirection.y, reflectedDirection.x) * Mathf.Rad2Deg;
                rb.rotation = angle - 90f; // корректируем угол на 90 градусов

                // Смещаем пулю чуть дальше точки столкновения, чтобы избежать повторного столкновения с той же точкой
                rb.position = hit.point + hit.normal * 0.01f;
            }
            else if (hit.collider != null && !hasExplode)
            {
                Explode();
            }
        }

        // Обновляем предыдущую позицию для следующего кадра
        previousPosition = rb.position;
    }

    private void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;

        // Создаем взрыв
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Наносим урон в радиусе взрыва
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(explosionDamage, knockbackForce);
            }
        }

        //playerWeaponController.CameraShake(cameraShakeDuration, cameraShakeMagnitude);

        Destroy(gameObject);
    }
}
