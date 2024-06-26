using UnityEngine;
using UnityEngine.UI;

public class CrystalCounterUI : MonoBehaviour
{
    public Image crystalIcon;           // Иконка кристаллов
    public Image[] digitImages;         // Массив Image для отображения цифр
    public Sprite[] digitSprites;       // Массив спрайтов для цифр (0-9)

    private void Start()
    {
        UpdateCrystalCount(0);  // Пример начального значения
    }

    public void UpdateCrystalCount(int crystalCount)
    {
        string crystalStr = crystalCount.ToString("D2");

        // Отображаем количество кристаллов
        for (int i = 0; i < digitImages.Length; i++)
        {
            int digit = int.Parse(crystalStr[i].ToString());
            digitImages[i].sprite = digitSprites[digit];
        }
    }
}
