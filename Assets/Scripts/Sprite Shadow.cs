using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 scale;
    [SerializeField] private Color shadowColor;
    [SerializeField] private int shadowOrder;
    [SerializeField] private GameObject prefab;
    [SerializeField] private bool createMask;

    private GameObject _prefabShadow;
    private SpriteRenderer[] _prefabSprRnd;
    
    private Transform _maskTransform;
    private SpriteMask _mask;
    private void Awake()
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
                    playerMovementScript.offset = new Vector2(offset.x, offset.y);
                    
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

    private void LateUpdate()
    {
        if(!createMask) return;
        _mask.sprite = _prefabSprRnd[0].sprite;
    }
}