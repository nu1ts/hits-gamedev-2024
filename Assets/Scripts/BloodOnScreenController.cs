using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodOnScreenController : MonoBehaviour
{
    public static BloodOnScreenController instance;
    public Image bloodImage; // Ссылка на UI элемент Image
    public float fadeDuration = 1.0f; // Длительность исчезновения

    private Color initialColor;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (bloodImage != null)
        {
            initialColor = bloodImage.color;
            initialColor.a = 0; // Устанавливаем начальную прозрачность в 0
            bloodImage.color = initialColor;
        }
    }

    public void ShowBloodEffect()
    {
        if (bloodImage != null)
        {
            StartCoroutine(FadeBloodEffect());
        }
    }

    private IEnumerator FadeBloodEffect()
    {
        float elapsedTime = 0;
        Color color = initialColor;
        color.a = 1; // Полностью непрозрачный

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            bloodImage.color = color;
            yield return null;
        }

        color.a = 0;
        bloodImage.color = color;
    }
}
