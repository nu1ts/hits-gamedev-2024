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
        RaycastHit2D hit = Physics2D.Raycast(previousPosition, transform.up, speed * Time.fixedDeltaTime);

        if (hit.collider != null && !hasDealtDamage)
        {
            // Наносим урон при столкновении
            DamageReceiver damageReceiver = hit.collider.GetComponent<DamageReceiver>();

            if (damageReceiver != null)
            {
                damageReceiver.TakeDamage(damage, knockbackForce);
                hasDealtDamage = true; // Устанавливаем флаг после нанесения урона
            }

            // Уничтожаем пулю
            Destroy(gameObject, destroyAfter);
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
