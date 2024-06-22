using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamageable
{
    [SerializeField] private int explosionDamage = 100;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private LayerMask damageLayerMask;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float cameraShakeDuration = 0.3f;
    [SerializeField] private float cameraShakeMagnitude = 0.5f;
    [SerializeField] private Collider2D hitCollider;
    [SerializeField] private Collider2D explosionCollider;

    private void Awake()
    {
        explosionCollider.enabled = false;
    }

    public void TakeDamage(int damage, float knockbackForce)
    {
        Explode();
    }

    private void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        explosionCollider.enabled = true;

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayerMask);
        foreach (Collider2D hit in hitObjects)
        {
            // Проверяем, является ли объект бочкой и взрываем его
            ExplosiveBarrel explosiveBarrel = hit.GetComponent<ExplosiveBarrel>();
            if (explosiveBarrel == this)
            {
                continue; // Продолжаем итерацию, чтобы не выполнять damageable.TakeDamage
            }

            // Если объект не является бочкой, наносим урон объектам, которые имеют интерфейс IDamageable
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(explosionDamage, 0f);
            }

        }

        GlobalEvents.TriggerCameraShake(cameraShakeDuration, cameraShakeMagnitude);

        Destroy(gameObject, 0.2f);
    }
}
