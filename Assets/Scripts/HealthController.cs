using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private ParticleSystem bloodParticleSystemWall;
    [SerializeField] private ParticleSystem bloodParticleSystemFloor;
    [SerializeField] private GameObject deadBodyPrefab; // Префаб трупа
    [SerializeField] private float knockbackDuration = 0.3f;
    private int currentHealth;
    private Rigidbody2D rb;

    public HealthBarUI healthBarUI;
    public GameObject spriteObject;
    public GameObject weaponObject;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = transform.root.GetComponent<Rigidbody2D>();

        if (healthBarUI != null)
        {
            healthBarUI.Initialize(maxHealth);
        }
    }

    public void TakeDamage(int damage, float knockbackForce)
    {
        currentHealth -= damage;

        Vector2 knockbackDirection = transform.up;
        StartCoroutine(ApplyKnockback(knockbackDirection, knockbackForce));

        //Debug.Log($"{gameObject.name} получил урон: {damage}. Текущие HP: {currentHealth}");

        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(currentHealth);
        }

        // Вызов эффекта крови
        if (currentHealth > 0)
        {
            BloodEffect();
        }

        if (currentHealth <= 0)
        {
            Die();
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
        //Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 0f);
        // Vector3 spawnPosition = transform.position - transform.up;
        // float rotationAngle = -transform.eulerAngles.z - 90f;
        // ParticleSystem instance = Instantiate(bloodParticleSystem, spawnPosition, Quaternion.Euler(rotationAngle, 90f, 90f));        //instance.transform.up = -transform.up;

        // // Запускаем систему частиц
        // instance.Play();

        // Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        if (this.gameObject.CompareTag("Player"))
        {
            BloodOnScreenController.instance.ShowBloodEffect();
        }

        if (bloodParticleSystemWall != null)
        {
            bloodParticleSystemWall.Play();
        }
        else
        {
            Debug.LogWarning("Blood ParticleSystem is not assigned in the inspector.");
        }

        if (bloodParticleSystemFloor != null)
        {
            bloodParticleSystemFloor.Play();
        }
        else
        {
            Debug.LogWarning("Blood ParticleSystem is not assigned in the inspector.");
        }
    }

    private void Die()
    {
        EnemyMovement enemyMovement = GetComponentInParent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.DisableParts();
        }

        GameObject deadBody = Instantiate(deadBodyPrefab, transform.position, transform.rotation);
        EnemyCorpseController enemyCorpseController = deadBody.GetComponent<EnemyCorpseController>();
        if (enemyCorpseController != null)
        {
            enemyCorpseController.Slide(transform.up);
        }

        EnemyManager.instance.UnregisterEnemy(transform.root.gameObject);
        DisableInteractions();
        Destroy(transform.root.gameObject, 3f);
        if (gameObject.CompareTag("Player"))
        {
            LevelController.instance.RestartLevel();
        }
        Destroy(gameObject);
    }

    private void DisableInteractions()
    {
        // Отключаем все коллайдеры
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        // Отключаем все скрипты, кроме тех, что управляют эффектами
        MonoBehaviour[] scripts = GetComponentsInParent<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (!(script is HealthController || script is EnemyCorpseController))
            {
                script.enabled = false;
            }
        }

        // Отключаем физическое взаимодействие
        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
        }

        Destroy(spriteObject);
        Destroy(weaponObject);
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
        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(currentHealth);
        }
    }
}
