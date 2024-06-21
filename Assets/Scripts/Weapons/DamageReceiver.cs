using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private HealthController health; // Ссылка на компонент здоровья корневого объекта

    public void TakeDamage(int damage, float knockbackForce)
    {
        if (health != null)
        {
            health.TakeDamage(damage, knockbackForce);
        }
        else
        {
            Debug.LogError("HealthController не найден.");
        }
    }
}