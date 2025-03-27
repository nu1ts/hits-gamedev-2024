using System.Collections;
using UnityEngine;

public class BloodParticleDestroy : MonoBehaviour
{
    public float fadeDuration = 5f; // Время, за которое кровь полностью исчезает
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        float startAlpha = spriteRenderer.color.a;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            Color tmpColor = spriteRenderer.color;
            tmpColor.a = Mathf.Lerp(startAlpha, 0, progress);
            spriteRenderer.color = tmpColor;

            progress += rate * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject); // Удаляем объект после исчезновения
    }
}
