using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float destroyAfter = 2f; // Время жизни пули
    public int damage = 10; // Урон, наносимый пулей
    public float knockbackForce = 1000f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;

        // Уничтожаем пулю через заданное время
        Destroy(gameObject, destroyAfter);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Урон наносится не через пулю, а через скрипт DamageReceiver.cs, который прикрепляется к частит тела объекта, которая должна получать урон

        // Уничтожаем пулю при столкновении с коллайдером
        //Destroy(gameObject);
    }
}
