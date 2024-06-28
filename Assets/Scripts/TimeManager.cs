using System.Collections;
using State_Machine;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public ColorGradingController colorGradingController;

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

    public void ActivateEffect(float duration)
    {
        if (colorGradingController != null)
        {
            StartCoroutine(EffectCoroutine(duration));
        }
    }

    private IEnumerator EffectCoroutine(float duration)
    {
        colorGradingController.enabled = true;
        colorGradingController.ActivateEffect();
        yield return new WaitForSecondsRealtime(duration);
        colorGradingController.enabled = false;
        colorGradingController.RestoreInitialValues();
    }

    public void SlowTime(float duration, float slowScale, PlayerMovement playerMovement)
    {
        StartCoroutine(SlowTimeCoroutine(duration, slowScale, playerMovement));
    }

    private IEnumerator SlowTimeCoroutine(float duration, float slowScale, PlayerMovement playerMovement)
    {
        float originalSpeed = playerMovement.speed;
        float originalAcceleration = playerMovement.acceleration;

        // Замедляем время
        Time.timeScale = slowScale;

        // Увеличиваем скорость игрока, но не чрезмерно
        //playerMovement.speed /= slowScale;
        playerMovement.acceleration /= slowScale;

        // Активация ускоренного эффекта
        ActivateEffect(duration);

        // Ждем указанное время
        yield return new WaitForSecondsRealtime(duration);

        // Возвращаем нормальную скорость времени
        Time.timeScale = 1f;

        // Возвращаем нормальную скорость игрока
        playerMovement.speed = originalSpeed;
        playerMovement.acceleration = originalAcceleration;
    }
}
