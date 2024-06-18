using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    public Vector2 offset;
    public Vector2 scale;
    public Color shadowColor;
    public int shadowOrder;
    public GameObject prefab;
    public bool mask;

    private GameObject _prefabShadow;
    private SpriteRenderer[] _prefabSprRnd;
    private SpriteMask[] _spriteMasks;
    public string[] spriteNames;
    
    public bool[] visibility;
    public bool[] hasMask;

    private void Awake()
    {
        InstantiatePrefab();
    }

    private void Start()
    {
        UpdateVisibility();
        CreateMasks();
        RemoveInvisibleObjects();
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
        
        if (visibility == null || visibility.Length != tempSprRnd.Length)
        {
            visibility = new bool[tempSprRnd.Length];
            for (var i = 0; i < tempSprRnd.Length; i++)
            {
                visibility[i] = tempSprRnd[i].enabled;
            }
        }

        if (hasMask != null && hasMask.Length == tempSprRnd.Length) return;
        hasMask = new bool[tempSprRnd.Length];
        for (var i = 0; i < tempSprRnd.Length; i++)
        {
            hasMask[i] = false;
        }
    }

    private void InstantiatePrefab()
    {
        if (!prefab) return;
        
        _prefabShadow = Instantiate(prefab);
        _prefabShadow.name = prefab.name + " Shadow";

        _prefabSprRnd = _prefabShadow.GetComponentsInChildren<SpriteRenderer>();
        _spriteMasks = new SpriteMask[_prefabSprRnd.Length];

        _prefabShadow.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y);
        _prefabShadow.transform.localScale = new Vector3(scale.x, scale.y);

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
                    playerMovementScript.crosshairOffset = new Vector2(offset.x, offset.y);
                }
                else
                {
                    Debug.LogWarning("PlayerMovement script is not attached to the prefab!");
                }
                break;
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
    
    private void CreateMasks()
    {
        if (_prefabSprRnd == null || hasMask == null || _prefabSprRnd.Length != hasMask.Length || !mask) return;
        
        for (var i = 0; i < _prefabSprRnd.Length; i++)
        {
            if (!hasMask[i]) return;
            _spriteMasks[i] = _prefabSprRnd[i].gameObject.AddComponent<SpriteMask>();
            _prefabSprRnd[i].maskInteraction = SpriteMaskInteraction.None;
        }
    }
    
    private void UpdateMasks()
    {
        if (_prefabSprRnd == null || _spriteMasks == null || _prefabSprRnd.Length != _spriteMasks.Length || !mask) return;

        for (var i = 0; i < _spriteMasks.Length; i++)
        {
            if (_spriteMasks[i])
            {
                _spriteMasks[i].sprite = _prefabSprRnd[i].sprite;
            }
        }
    }
    
    private void RemoveInvisibleObjects()
    {
        if (_prefabSprRnd == null) return;

        foreach (var spriteRenderer in _prefabSprRnd)
        {
            if (!spriteRenderer.enabled)
            {
                Destroy(spriteRenderer.gameObject);
            }
        }
    }

    private void LateUpdate()
    {
        UpdateMasks();
    }
}