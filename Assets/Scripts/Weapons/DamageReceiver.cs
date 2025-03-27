using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    private IDamageable damageable; // Ссылка на компонент здоровья корневого объекта

    private void Awake()
    {
        damageable = GetComponentInParent<IDamageable>();
    }

    public void TakeDamage(int damage, float knockbackForce)
    {
        if (damageable != null)
        {
            damageable.TakeDamage(damage, knockbackForce);
        }
        else
        {
            Debug.LogError("HealthController не найден.");
        }
    }
}