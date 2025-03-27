using UnityEngine;
using UnityEngine.UI;

public class AmmoCounterUI : MonoBehaviour
{
    public Image ammoIcon;            // Иконка патронов
    public Image[] digitImages;       // Массив Image для отображения цифр
    //public GameObject digitPrefab; 
    public Sprite[] digitSprites;     // Массив спрайтов для цифр (0-9)
    public Sprite ammoIconFull;
    public Sprite ammoIcon2Third;
    public Sprite ammoIcon1Third;
    public Sprite ammoIconEmpty;
    
    private void Start()
    {
        UpdateAmmoCount(30, 30);  // Пример начального значения
    }

    public void UpdateAmmoCount(int ammoCount, int maxAmmo)
    {
        string ammoStr = ammoCount.ToString("D2");
        string maxAmmoStr = maxAmmo.ToString("D2");

        // Отображаем текущее количество патронов
        for (int i = 0; i < digitImages.Length / 2; i++)
        {
            int digitIndex = i;
            if (i < ammoStr.Length)
            {
                int digit = int.Parse(ammoStr[i].ToString());
                digitImages[digitIndex].sprite = digitSprites[digit];
                digitImages[digitIndex].enabled = true;
            }
            else
            {
                digitImages[digitIndex].enabled = false;
            }
        }

        // Отображаем максимальное количество патронов
        for (int i = 0; i < digitImages.Length / 2; i++)
        {
            int digitIndex = i + digitImages.Length / 2; // Сдвигаем индексы для maxAmmo
            if (i < maxAmmoStr.Length)
            {
                int digit = int.Parse(maxAmmoStr[i].ToString());
                digitImages[digitIndex].sprite = digitSprites[digit];
                digitImages[digitIndex].enabled = true;
            }
            else
            {
                digitImages[digitIndex].enabled = false;
            }
        }

        // Определяем спрайт иконки патронов в зависимости от количества патронов
        if (ammoCount <= 0)
        {
            ammoIcon.sprite = ammoIconEmpty;
        }
        else if (ammoCount <= maxAmmo / 3)
        {
            ammoIcon.sprite = ammoIcon1Third;
        }
        else if (ammoCount <= 2 * maxAmmo / 3)
        {
            ammoIcon.sprite = ammoIcon2Third;
        }
        else
        {
            ammoIcon.sprite = ammoIconFull;
        }
    }

    // public void UpdateAmmoCount(int ammoCount)
    // {
    //     // Удаляем предыдущие цифры
    //     foreach (Transform child in transform)
    //     {
    //         Destroy(child.gameObject);
    //     }

    //     string ammoStr = ammoCount.ToString();

    //     // Создаём новые цифры
    //     for (int i = 0; i < ammoStr.Length; i++)
    //     {
    //         int digit = int.Parse(ammoStr[i].ToString());
    //         GameObject digitObject = Instantiate(digitPrefab, transform);
    //         Image digitImage = digitObject.GetComponent<Image>();
    //         digitImage.sprite = digitSprites[digit];
    //     }
    // }
}
