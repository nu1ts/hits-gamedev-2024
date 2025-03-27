using UnityEngine;
using UnityEngine.UI;

public class DashBarUI : MonoBehaviour
{
    public Image dashFillImage; // Ссылка на Image компонента, который заполняет полосу кулдауна
    private float maxCooldown;
    private float currentCooldown;

    public void Initialize(float maxCooldown)
    {
        this.maxCooldown = maxCooldown;
        this.currentCooldown = 0f;
        UpdateDashBar();
    }

    public void UpdateCooldown(float currentCooldown)
    {
        this.currentCooldown = currentCooldown;
        UpdateDashBar();
    }

    private void UpdateDashBar()
    {
        float fillAmount = 1f - (currentCooldown / maxCooldown);

        // Отображаем заполнение скачками на 1/3, 2/3 и полностью
        if (fillAmount <= 0.33f)
        {
            dashFillImage.fillAmount = 0f;
        }
        else if (fillAmount <= 0.66f)
        {
            dashFillImage.fillAmount = 1f / 3f;
        }
        else if (fillAmount < 1f)
        {
            dashFillImage.fillAmount = 2f / 3f;
        }
        else
        {
            dashFillImage.fillAmount = 3f / 3f;
        }
    }
}
