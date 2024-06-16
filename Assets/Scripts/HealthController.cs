using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    //[SerializeField] private GameObject bloodEffectPrefab; // Префаб эффекта крови
    [SerializeField] private ParticleSystem bloodParticleSystem;
    [SerializeField] private GameObject deadBodyPrefab; // Префаб трупа
     [SerializeField] private float knockbackDuration = 0.3f;
    private int currentHealth;
    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = transform.root.GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, float knockbackForce)
    {
        currentHealth -= damage;

        Vector2 knockbackDirection = -transform.up;
        StartCoroutine(ApplyKnockback(knockbackDirection, knockbackForce));

        Debug.Log($"{gameObject.name} получил урон: {damage}. Текущие HP: {currentHealth}");

        // Вызов эффекта крови
        if (currentHealth > 0)
        {
            BloodEffect();
        }

        if (currentHealth <= 0)
        {
            Die(knockbackForce);
        }
    }

    private IEnumerator ApplyKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        float elapsedTime = 0;
        while (elapsedTime < knockbackDuration)
        {
            rb.MovePosition(rb.position + knockbackDirection * knockbackForce * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private void BloodEffect()
    {
        // Создание эффекта крови
        // Предполагается, что у вас есть префаб эффекта крови, который вы можете использовать
        // Например:
        // Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
        Instantiate(bloodParticleSystem, transform.position, Quaternion.Euler(90f, 0f, 0f));
    }

    private void Die(float knockbackForce)
    {
        // Логика смерти
        BloodEffect();

        // Создание спрайта мёртвого тела
        GameObject deadBody = Instantiate(deadBodyPrefab, transform.position, transform.rotation);
        EnemyCorpseController enemyCorpseController = deadBody.GetComponent<EnemyCorpseController>();
        if (enemyCorpseController != null)
        {
            // Применение силы отбрасывания к мёртвому телу
            //StartCoroutine(SlideDeadBody(deadBodyController, -transform.up));
            enemyCorpseController.Slide(-transform.up);
        }

        Destroy(transform.root.gameObject);
    }

    private IEnumerator SlideDeadBody(EnemyCorpseController enemyCorpseController, Vector2 slideDirection)
    {
        // Запускаем скольжение мертвого тела
        enemyCorpseController.Slide(slideDirection);
        yield return null; // Даем возможность корутине запуститься
    }
    
    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}
