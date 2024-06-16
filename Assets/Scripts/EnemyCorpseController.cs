using System.Collections;
using UnityEngine;

public class EnemyCorpseController : MonoBehaviour
{
    [SerializeField] private float slideDuration = 2f; // Длительность скольжения тела
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on EnemyCorpseController object.");
        }
    }

    public void Slide(Vector2 slideDirection)
    {
        StartCoroutine(SlideCoroutine(slideDirection));
    }

    private IEnumerator SlideCoroutine(Vector2 slideDirection)
    {
        float elapsedTime = 0;
        while (elapsedTime < slideDuration)
        {
            rb.MovePosition(rb.position + slideDirection * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
