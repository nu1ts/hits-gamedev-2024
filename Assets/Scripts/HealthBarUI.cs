using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthFillImage; // Ссылка на Image компонента, который заполняет полосу здоровья
    private int maxHealth;
    private int currentHealth;

    public void Initialize(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void UpdateHealth(int currentHealth)
    {
        this.currentHealth = currentHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float fillAmount = (float)currentHealth / maxHealth;
        healthFillImage.fillAmount = fillAmount;
    }
}
