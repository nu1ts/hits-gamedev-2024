using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 scale;
    public Color shadowColor;
    public int shadowOrder;
    public GameObject prefab;
    public bool mask;

    private List<SpriteRenderer> _shadowSprRnd;
    private List<SpriteMask> _shadowMasks;
    private List<Transform> _shadowTransforms;
    private Dictionary<string, Transform> _parentTransforms;
    public string[] spriteNames;
    public bool[] visibility;
    public bool[] hasMask;

    private void Awake()
    {
        _parentTransforms = new Dictionary<string, Transform>();
        foreach (Transform child in transform)
        {
            _parentTransforms[child.name] = child;
        }

        InstantiateShadows();
    }

    private void OnValidate()
    {
        if (!prefab) return;

        var tempSprRnd = prefab.GetComponentsInChildren<SpriteRenderer>();

        spriteNames = tempSprRnd.Select(sr => sr.gameObject.name).ToArray();

        if (visibility == null || visibility.Length != tempSprRnd.Length)
        {
            visibility = tempSprRnd.Select(sr => sr.enabled).ToArray();
        }

        if (hasMask == null || hasMask.Length != tempSprRnd.Length)
        {
            hasMask = new bool[tempSprRnd.Length];
        }
    }

    private void InstantiateShadows()
    {
        if (!prefab) return;

        _shadowSprRnd = new List<SpriteRenderer>();
        _shadowTransforms = new List<Transform>();

        foreach (Transform prefabChild in prefab.transform)
        {
            var parentChild = transform.Find(prefabChild.name);
            var childObject = Instantiate(prefabChild, parentChild);

            if (shadowOrder == 0)
            {
                childObject.SetParent(transform);
                _shadowTransforms.Add(childObject.transform);
            }

            RemoveComponents(childObject.gameObject);

            _shadowSprRnd.AddRange(childObject.GetComponentsInChildren<SpriteRenderer>());

            childObject.name = prefabChild.name + " Shadow";
            childObject.tag = "Shadow";
        }

        foreach (var spriteRenderer in _shadowSprRnd)
        {
            spriteRenderer.transform.localScale += scale;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            spriteRenderer.color = shadowColor;
            spriteRenderer.sortingOrder = shadowOrder;
        }

        UpdateVisibility();
        CreateMasks();
    }

    private static void RemoveComponents(GameObject obj)
    {
        var components = obj.GetComponents<Component>();

        foreach (var component in components)
        {
            if (component is not SpriteRenderer && 
                component is not SpriteMask && 
                component is not Transform && 
                component is not Animator)
            {
                Destroy(component);
            }
        }
    }

    private void UpdateVisibility()
    {
        if (_shadowSprRnd == null || visibility == null || _shadowSprRnd.Count != visibility.Length) return;

        foreach (var spriteRenderer in _shadowSprRnd)
        {
            spriteRenderer.gameObject.SetActive(visibility[_shadowSprRnd.IndexOf(spriteRenderer)]);
        }

        RemoveInvisibleObjects();
    }

    private void CreateMasks()
    {
        if (_shadowSprRnd == null || hasMask == null || _shadowSprRnd.Count != hasMask.Length || !mask) return;

        _shadowMasks = new List<SpriteMask>();

        for (var i = 0; i < _shadowSprRnd.Count; i++)
        {
            if (hasMask[i])
            {
                var spriteMask = _shadowSprRnd[i].gameObject.AddComponent<SpriteMask>();
                _shadowMasks.Add(spriteMask);
                _shadowSprRnd[i].maskInteraction = SpriteMaskInteraction.None;
            }
        }
    }

    private void UpdateMasks()
    {
        if (_shadowMasks == null) return;

        for (var i = 0; i < _shadowMasks.Count; i++)
        {
            if (_shadowMasks[i])
            {
                _shadowMasks[i].sprite = _shadowSprRnd[i].sprite;
            }
        }
    }

    private void RemoveInvisibleObjects()
    {
        if (_shadowSprRnd == null) return;

        var invisibleObjects = _shadowSprRnd.Where(sr => !sr.gameObject.activeSelf).ToList();

        foreach (var spriteRenderer in invisibleObjects)
        {
            Destroy(spriteRenderer.gameObject);
        }
    }

    private void UpdateTransforms()
    {
        foreach (var transformChild in _shadowTransforms)
        {
            var parentName = transformChild.name.Replace(" Shadow", "");
            
            if (!_parentTransforms.TryGetValue(parentName, out var parentChild))
            {
                continue;
            }
            
            transformChild.position = parentChild.position + offset;
            transformChild.rotation = parentChild.rotation;
            transformChild.localScale = parentChild.localScale + scale;
        }
    }

    private void Update()
    {
        UpdateMasks();

        if (shadowOrder == 0)
        {
            UpdateTransforms();
        }
    }
}