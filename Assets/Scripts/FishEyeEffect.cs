using UnityEngine;

[ExecuteInEditMode]
public class FishEyeEffect : MonoBehaviour
{
    public Material fishEyeMaterial;
    [Range(0, 1)]
    public float strength = 0.5f;
    [Range(0.5f, 1.0f)]
    public float zoom = 0.75f; // Добавлено новое свойство для управления зумом

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (fishEyeMaterial != null)
        {
            fishEyeMaterial.SetFloat("_Strength", strength);
            fishEyeMaterial.SetFloat("_Zoom", zoom); // Передаем значение зума в материал
            Graphics.Blit(source, destination, fishEyeMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
