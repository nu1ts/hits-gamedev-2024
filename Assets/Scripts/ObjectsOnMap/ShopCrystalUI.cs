using UnityEngine;
using UnityEngine.UI;

public class ShopCrystalUI : MonoBehaviour
{
    public Image[] digitImages;       // Массив Image для отображения цифр
    public Sprite[] digitSprites;     // Массив спрайтов для цифр (0-9)

    private int currentPrice = 10;    // Пример начальной цены
    private Canvas canvas;


    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        UpdateCrystalCount(currentPrice);  // Обновляем отображение начальной цены
    }

    public void SetActive(bool isActive)
    {
        canvas.enabled = isActive;
    }

    public void UpdateCrystalCount(int crystals)
    {
        string crystalStr = crystals.ToString();

        // Отображаем цифры цены
        for (int i = 0; i < digitImages.Length; i++)
        {
            if (i < crystalStr.Length)
            {
                int digit = int.Parse(crystalStr[i].ToString());
                digitImages[i].sprite = digitSprites[digit];
                digitImages[i].enabled = true;
            }
            else
            {
                digitImages[i].enabled = false;
            }
        }
    }
}
