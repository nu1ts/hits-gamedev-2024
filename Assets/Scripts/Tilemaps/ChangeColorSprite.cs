using UnityEngine;

public class ChangeColorSprite : MonoBehaviour
{
public Color targetColor = Color.magenta;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = targetColor;
        }
    }
}
