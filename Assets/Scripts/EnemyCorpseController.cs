using System.Collections;
using UnityEngine;

public class EnemyCorpseController : MonoBehaviour
{
    [SerializeField] private float slideDuration = 2f; // Длительность скольжения тела
    [SerializeField] private float destroyAfter = 2f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Slide(Vector2 slideDirection)
    {
        StartCoroutine(SlideCoroutine(slideDirection));

        Destroy(gameObject, destroyAfter);
    }

    private IEnumerator SlideCoroutine(Vector2 slideDirection)
    {
        float elapsedTime = 0;
        while (elapsedTime < slideDuration)
        {
            rb.MovePosition(rb.position + slideDirection * Time.fixedDeltaTime * 3);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
