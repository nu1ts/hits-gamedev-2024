using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ColorGradingController : MonoBehaviour
{
    public PostProcessProfile profile; // Ссылка на профиль Post Processing
    private ColorGrading colorGrading;
    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;

    public float hueShiftSpeed = 1.0f; // Скорость изменения Hue Shift
    public float minDistortionIntensity = 10f; // Минимальная интенсивность Lens Distortion
    public float maxDistortionIntensity = 20f; // Максимальная интенсивность Lens Distortion
    public float distortionSpeedMultiplier = 2.0f; // Множитель скорости изменения Distortion Intensity

    private float initialHueShift;
    private float initialDistortionIntensity;
    private float initialAberrationIntensity;
    private bool isEffectActive = false;

    private void Start()
    {
        // Получаем компоненты ColorGrading, LensDistortion и ChromaticAberration из PostProcessProfile
        if (profile != null)
        {
            if (!profile.TryGetSettings(out colorGrading))
                Debug.LogError("Color Grading component not found in the assigned Post Process Profile!");

            if (!profile.TryGetSettings(out lensDistortion))
                Debug.LogError("Lens Distortion component not found in the assigned Post Process Profile!");

            if (!profile.TryGetSettings(out chromaticAberration))
                Debug.LogError("Chromatic Aberration component not found in the assigned Post Process Profile!");

            // Сохраняем начальные значения эффектов
            if (colorGrading != null)
                initialHueShift = colorGrading.hueShift.value;

            if (lensDistortion != null)
                initialDistortionIntensity = lensDistortion.intensity.value;

            if (chromaticAberration != null)
                initialAberrationIntensity = chromaticAberration.intensity.value;
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
        if (lensDistortion != null && isEffectActive)
        {
            float distortionIntensity = Mathf.Lerp(minDistortionIntensity, maxDistortionIntensity, Mathf.PingPong(Time.time * distortionSpeedMultiplier, 1f));
            lensDistortion.intensity.value = distortionIntensity;
        }

        // Устанавливаем максимальную интенсивность для Chromatic Aberration
        if (chromaticAberration != null && isEffectActive)
        {
            chromaticAberration.intensity.value = 1f;
        }
    }

    public void RestoreInitialValues()
    {
        if (colorGrading != null)
        {
            colorGrading.hueShift.value = initialHueShift;
        }

        if (lensDistortion != null)
        {
            lensDistortion.intensity.value = initialDistortionIntensity;
        }

        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = initialAberrationIntensity;
        }

        isEffectActive = false;
    }

    public void ActivateEffect()
    {
        isEffectActive = true;
    }
}
