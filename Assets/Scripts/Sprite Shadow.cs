using Unity.VisualScripting;
using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 scale;
    [SerializeField] private Color shadowColor;
    [SerializeField] private int shadowOrder;

    private SpriteRenderer _spriteSprRnd;
    private SpriteRenderer _shadowSprRnd;
    private Transform _shadowTransform;
    private SpriteMask _spriteMask;
    private Transform _maskTransform;
    private void Start()
    {
        _spriteSprRnd = GetComponent<SpriteRenderer>();
        
        _shadowTransform = new GameObject().transform;
        _shadowTransform.parent = transform;
        _shadowTransform.gameObject.name = "Shadow";

        _shadowSprRnd = _shadowTransform.gameObject.AddComponent<SpriteRenderer>();
        _shadowSprRnd.color = shadowColor;
        _shadowSprRnd.sortingOrder = shadowOrder;
        
        _maskTransform = transform.Find("Shadow Mask");
        if (_maskTransform)
        {
            _spriteMask = _maskTransform.GetComponent<SpriteMask>();
        }
        else
        {
            _maskTransform = new GameObject().transform;
            _maskTransform.parent = transform;
            _maskTransform.gameObject.name = "Shadow Mask";
            _spriteMask = _maskTransform.gameObject.AddComponent<SpriteMask>();
        }
        
        if(shadowOrder == -1) _shadowSprRnd.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }
    
    private void LateUpdate()
    {
        _shadowTransform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y);
        _shadowTransform.localScale = new Vector3(scale.x, scale.y);
        _shadowSprRnd.sprite = _spriteSprRnd.sprite;
        
        if (shadowOrder == -1) return;
        if(!_maskTransform) return;
        _maskTransform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y);
        _maskTransform.localScale = new Vector3(scale.x, scale.y);
        _spriteMask.sprite = _spriteSprRnd.sprite;
    }
}
