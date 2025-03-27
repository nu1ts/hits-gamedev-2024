using UnityEngine;
using UnityEngine.UI;

public class PlayerCrystalController : MonoBehaviour
{
    public int crystalCount = 0;
    public CrystalCounterUI crystalCounterUI; // Ссылка на UI для отображения кристаллов

    private void Start()
    {
        UpdateCrystalUI();
    }

    public void UpdateCrystalUI()
    {
        if (crystalCounterUI != null)
        {
            crystalCounterUI.UpdateCrystalCount(crystalCount);
        }
    }

    public void AddCrystals(int amount)
    {
        crystalCount += amount;
        UpdateCrystalUI();
    }
}
