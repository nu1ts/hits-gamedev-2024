using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ColorGradingController : MonoBehaviour
{
    public PostProcessProfile profile; // Ссылка на профиль Post Processing
    private ColorGrading colorGrading;
    private LensDistortion lensDistortion;

    public float hueShiftSpeed = 1.0f; // Скорость изменения Hue Shift
    public float minDistortionIntensity = 10f; // Минимальная интенсивность Lens Distortion
    public float maxDistortionIntensity = 20f; // Максимальная интенсивность Lens Distortion

    private void Start()
    {
        // Получаем компоненты ColorGrading и LensDistortion из PostProcessProfile
        if (profile != null)
        {
            profile.TryGetSettings(out colorGrading);
            profile.TryGetSettings(out lensDistortion);

            if (colorGrading == null)
            {
                Debug.LogError("Color Grading component not found in the assigned Post Process Profile!");
            }

            if (lensDistortion == null)
            {
                Debug.LogError("Lens Distortion component not found in the assigned Post Process Profile!");
            }
        }
        else
        {
            Debug.LogError("Post Process Profile is not assigned!");
        }
    }

    private void Update()
    {
        // Изменяем параметр Hue Shift со временем
        if (colorGrading != null)
        {
            float hueShift = Mathf.Sin(Time.time * hueShiftSpeed) * 180f; // Пример изменения по синусоиде
            colorGrading.hueShift.value = hueShift;
        }

        // Изменяем параметр Lens Distortion Intensity в пределах заданных значений
        if (lensDistortion != null)
        {
            float distortionIntensity = Mathf.Lerp(minDistortionIntensity, maxDistortionIntensity, Mathf.PingPong(Time.time, 1f));
            lensDistortion.intensity.value = distortionIntensity;
        }
    }
}
