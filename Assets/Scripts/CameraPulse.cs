using UnityEngine;
using System.Collections;

public class CameraPulse : MonoBehaviour
{
    public Camera mainCamera; // Ссылка на основную камеру
    public float pulseStrength = 5f; // Сила пульсации (на сколько изменяется поле зрения)
    public float pulseDuration = 0.1f; // Длительность одного пульса
    public float pulseInterval = 1f; // Интервал между пульсациями

    private float originalFOV; // Оригинальное поле зрения камеры

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Если камера не назначена, использовать основную камеру
        }
        originalFOV = mainCamera.fieldOfView; // Сохранить оригинальное поле зрения камеры
        StartCoroutine(PulseRoutine()); // Запустить корутину для пульсации камеры
    }

    private IEnumerator PulseRoutine()
    {
        while (true) // Бесконечный цикл для постоянной пульсации
        {
            // Сначала плавно увеличиваем поле зрения
            float elapsedTime = 0f;
            while (elapsedTime < pulseDuration)
            {
                mainCamera.fieldOfView = Mathf.Lerp(originalFOV, originalFOV + pulseStrength, elapsedTime / pulseDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Затем плавно уменьшаем поле зрения обратно
            elapsedTime = 0f;
            while (elapsedTime < pulseDuration)
            {
                mainCamera.fieldOfView = Mathf.Lerp(originalFOV + pulseStrength, originalFOV, elapsedTime / pulseDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            mainCamera.fieldOfView = originalFOV; // Восстановить оригинальное значение FOV на случай ошибок
            yield return new WaitForSeconds(pulseInterval); // Ждать перед следующим пульсом
        }
    }
}
