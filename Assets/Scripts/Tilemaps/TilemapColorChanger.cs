using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapColorChanger : MonoBehaviour
{
    public Tilemap tilemap;
    public Material tilemapMaterial;

    public GameObject floor;
    public Material floorMaterial;

    private void Start()
    {
        if (tilemap == null || tilemapMaterial == null)
        {
            Debug.LogError("Tilemap or Material is not assigned!");
            return;
        }

        if (floor == null || floorMaterial == null)
        {
            Debug.LogError("Tilemap or Material is not assigned!");
            return;
        }

        tilemap.GetComponent<Renderer>().material = tilemapMaterial;
        floor.GetComponent<Renderer>().material = floorMaterial;
        ApplyMaterial();
    }

    public void ApplyMaterial()
    {
        var tilemapRenderer = tilemap.GetComponent<TilemapRenderer>();
        tilemapRenderer.material = tilemapMaterial;
        tilemapRenderer.sortingOrder = 1; // Убедитесь, что ваш Tilemap выше пола
    }

    public void ChangeColors(Color color1, Color color2, Color color3, Color color4, Color color5)
    {
        tilemapMaterial.SetColor("_TargetColor1", color1);
        tilemapMaterial.SetColor("_TargetColor2", color2);
        tilemapMaterial.SetColor("_TargetColor3", color3);
        tilemapMaterial.SetColor("_TargetColor4", color4);
        floorMaterial.SetColor("_TargetColor", color5);
    }

    public void ChangeColors(string hexColor1, string hexColor2, string hexColor3, string hexColor4, string hexColor5)
    {
        ChangeColors(
            HexToColor(hexColor1),
            HexToColor(hexColor2),
            HexToColor(hexColor3),
            HexToColor(hexColor4),
            HexToColor(hexColor5)
        );
    }

    private Color HexToColor(string hex)
    {
        hex = hex.Replace("#", "");
        if (hex.Length != 6) throw new System.Exception("Invalid hex color format. It should be in the form #RRGGBB.");
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
}
