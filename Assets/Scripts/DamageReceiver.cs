using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private HealthController health; // Ссылка на компонент здоровья корневого объекта

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null)
        {
            health.TakeDamage(bullet.damage, bullet.knockbackForce);
            Destroy(bullet.gameObject);
        }
    }
}
