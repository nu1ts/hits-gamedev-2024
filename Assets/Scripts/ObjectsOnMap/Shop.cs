using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text costText; // Text component to show the cost of the weapon
    public int weaponCost; // Cost of the weapon
    public BasicWeapon weaponPrefab; // Prefab of the weapon to be sold
    public Sprite inactiveSprite; // Sprite to show when the shop item is inactive

    private PlayerCrystalController playerController;
    private PlayerWeaponController playerWeaponController;
    private bool playerInRange = false;
    private bool isActive = true;
    private SpriteRenderer shopSpriteRenderer;
    private ShopCrystalUI shopCrystalUI;
    public int weaponPrice = 10;

    private void Start()
    {
        //costText.text = weaponCost + " Crystals";

        // Get the sprite from the weapon prefab
        if (weaponPrefab != null)
        {
            SpriteRenderer weaponSpriteRenderer = weaponPrefab.GetComponentInChildren<SpriteRenderer>();
            if (weaponSpriteRenderer != null)
            {
                // Find the WeaponSprite child and set its sprite
                Transform weaponSpriteTransform = transform.Find("WeaponSprite");
                if (weaponSpriteTransform != null)
                {
                    SpriteRenderer shopItemSpriteRenderer = weaponSpriteTransform.GetComponent<SpriteRenderer>();
                    if (shopItemSpriteRenderer != null)
                    {
                        shopItemSpriteRenderer.sprite = weaponSpriteRenderer.sprite;
                    }
                }
            }
        }

        shopSpriteRenderer = GetComponent<SpriteRenderer>();

        shopCrystalUI = GetComponent<ShopCrystalUI>();
        if (shopCrystalUI != null)
        {
            shopCrystalUI.UpdateCrystalCount(weaponCost);
        }

        UpdateCrystalCounter();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            TryPurchaseWeapon();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            playerController = collision.GetComponentInParent<PlayerCrystalController>();
            playerWeaponController = collision.GetComponentInParent<PlayerWeaponController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            playerController = null;
            playerWeaponController = null;
        }
    }

    private void TryPurchaseWeapon()
    {
        if (playerController != null && playerWeaponController != null && playerController.crystalCount >= weaponCost)
        {
            playerController.crystalCount -= weaponCost;
            //playerController.UpdateCrystalUI();
            playerWeaponController.EquipWeapon(weaponPrefab);
            SetInactive();
        }
    }

    private void SetInactive()
    {
        isActive = false;
        //costText.text = "Sold Out";
        if (inactiveSprite != null)
        {
            shopSpriteRenderer.sprite = inactiveSprite;
        }

        // Disable the collider
        GetComponent<Collider2D>().enabled = false;

        // Stop the ParticleSystem
        Transform weaponSpriteTransform = transform.Find("WeaponSprite");
        if (weaponSpriteTransform != null)
        {
            weaponSpriteTransform.gameObject.SetActive(false);
        }

        if (shopCrystalUI != null)
        {
            shopCrystalUI.SetActive(false); // Скрыть UI магазина
        }
    }

    private void UpdateCrystalCounter()
    {
        shopCrystalUI.UpdateCrystalCount(weaponPrice);
    }
}