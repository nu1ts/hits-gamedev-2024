using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float destroyAfter = 2f; // Время жизни пули
    public int damage = 10; // Урон, наносимый пулей
    public float knockbackForce = 1000f;

    private Rigidbody2D rb;
    private Vector2 previousPosition;
    private bool hasDealtDamage = false;

    public LayerMask collisionLayers;
    public LayerMask teleportLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;

        // Запоминаем начальную позицию пули
        previousPosition = rb.position;

        // Уничтожаем пулю через заданное время
        Destroy(gameObject, destroyAfter);
    }

    private void FixedUpdate()
    {
        // Проверка пересечения пули с объектами
        RaycastHit2D hit = Physics2D.Raycast(previousPosition, transform.up, speed * Time.fixedDeltaTime, collisionLayers);

        if (hit.collider != null)
        {
            if (((1 << hit.collider.gameObject.layer) & teleportLayer) != 0)
            {
                // Обрабатываем телепортацию
                Teleport teleport = hit.collider.GetComponent<Teleport>();
                if (teleport != null && teleport.linkedTeleport != null)
                {
                    transform.position = teleport.linkedTeleport.transform.position;
                    previousPosition = rb.position;
                }
            }
            else if (hit.collider.CompareTag("Wall"))
            {
                Vector2 reflectedDirection = Vector2.Reflect(rb.velocity.normalized, hit.normal);

                // Применяем отраженное направление пули
                rb.velocity = reflectedDirection * speed;

                // Разворачиваем пулю в сторону отражения
                float angle = Mathf.Atan2(reflectedDirection.y, reflectedDirection.x) * Mathf.Rad2Deg;
                rb.rotation = angle - 90f; // корректируем угол на 90 градусов

                // Смещаем пулю чуть дальше точки столкновения, чтобы избежать повторного столкновения с той же точкой
                rb.position = hit.point + hit.normal * 0.01f;
                //Destroy(gameObject);
            }
            else if (hit.collider != null && !hasDealtDamage)
            {
                // Наносим урон при столкновении
                IDamageable damageReceiver = hit.collider.GetComponent<IDamageable>();

                if (damageReceiver != null)
                {
                    damageReceiver.TakeDamage(damage, knockbackForce);
                    hasDealtDamage = true; // Устанавливаем флаг после нанесения урона
                    Destroy(gameObject);
                }
            }
        }

        // Обновляем предыдущую позицию для следующего кадра
        previousPosition = rb.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasDealtDamage) return; // Выход если урон уже нанесен

        DamageReceiver damageReceiver = collision.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            damageReceiver.TakeDamage(damage, knockbackForce);
            hasDealtDamage = true; // Устанавливаем флаг после нанесения урона
            Destroy(gameObject);
        }
    }
}
