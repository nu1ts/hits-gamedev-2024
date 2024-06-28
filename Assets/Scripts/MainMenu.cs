using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("CLICKED");
        StartCoroutine(FadeOutCanvas(GetComponentInParent<CanvasGroup>()));
    }

    // Метод для открытия опций
    public void OpenOptions()
    {
        // Здесь будет логика для открытия опций
    }

    // Метод для выхода из игры
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private IEnumerator FadeOutCanvas(CanvasGroup canvasGroup)
    {
        float duration = 1f; // Длительность исчезновения
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // После завершения исчезновения, выполняем переход на уровень
        LevelController.instance.FirstLevel(); // замените "GameScene" на имя вашей игровой сцены
    }
}
