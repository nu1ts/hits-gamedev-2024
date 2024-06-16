using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float destroyAfter = 2f; // Время жизни пули

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
        // Уничтожаем пулю при столкновении с коллайдером
        Destroy(gameObject);
    }
}
