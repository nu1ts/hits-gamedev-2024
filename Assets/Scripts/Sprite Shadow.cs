using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    public Vector2 offset;
    public Vector2 scale;
    public Color shadowColor;
    public int shadowOrder;
    public GameObject prefab;
    public bool createMask;

    private GameObject _prefabShadow;
    private SpriteRenderer[] _prefabSprRnd;
    public string[] spriteNames;
    
    public bool[] visibility;

    private Transform _maskTransform;
    private SpriteMask _mask;

    private void Awake()
    {
        InstantiatePrefab();
    }

    private void Start()
    {
        UpdateVisibility();
    }

    private void OnValidate()
    {
        if (!prefab) return;
        
        var tempSprRnd = prefab.GetComponentsInChildren<SpriteRenderer>();
        
        spriteNames = new string[tempSprRnd.Length];
        for (var i = 0; i < tempSprRnd.Length; i++)
        {
            spriteNames[i] = tempSprRnd[i].gameObject.name;
        }
        
        if (visibility != null && visibility.Length == tempSprRnd.Length) return;
        visibility = new bool[tempSprRnd.Length];
        for (var i = 0; i < tempSprRnd.Length; i++)
        {
            visibility[i] = tempSprRnd[i].enabled;
        }
    }

    private void InstantiatePrefab()
    {
        if (prefab)
        {
            _prefabShadow = Instantiate(prefab);
            _prefabShadow.name = prefab.name + " Shadow";

            _prefabSprRnd = _prefabShadow.GetComponentsInChildren<SpriteRenderer>();

            _prefabShadow.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y);
            _prefabShadow.transform.localScale = new Vector3(scale.x, scale.y);

            if (_prefabSprRnd == null) return;
            foreach (var spriteRenderer in _prefabSprRnd)
            {
                spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                spriteRenderer.color = shadowColor;
                spriteRenderer.sortingOrder = shadowOrder;
            }

            switch (prefab.name)
            {
                case "Player":
                    var playerMovementScript = _prefabShadow.GetComponent<PlayerMovement>();
                    if (playerMovementScript != null)
                    {
                        playerMovementScript.offset = new Vector2(offset.x, offset.y);
                    }
                    else
                    {
                        Debug.LogWarning("PlayerMovement script is not attached to the prefab!");
                    }

                    if (!createMask) return;
                    _mask = _prefabShadow.transform.Find("Head").gameObject.AddComponent<SpriteMask>();
                    _prefabSprRnd[0].maskInteraction = SpriteMaskInteraction.None;
                    break;

                default:
                    Debug.LogError("Unhandled prefab name: " + prefab.name);
                    break;
            }
        }
        else
        {
            Debug.LogError("There's no prefab to instantiate!");
        }
    }

    private void UpdateVisibility()
    {
        if (_prefabSprRnd == null || visibility == null || _prefabSprRnd.Length != visibility.Length) return;
        for (var i = 0; i < _prefabSprRnd.Length; i++)
        {
            _prefabSprRnd[i].enabled = visibility[i];
        }
    }

    private void LateUpdate()
    {
        if (!createMask && prefab) return;
        _mask.sprite = _prefabSprRnd[0].sprite;
    }
}