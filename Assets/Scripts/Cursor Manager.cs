using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    private Vector2 _cursorHotspot;
    private void Start()
    {
        _cursorHotspot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);
        Cursor.SetCursor(cursorTexture, _cursorHotspot, CursorMode.Auto);
    }
}
