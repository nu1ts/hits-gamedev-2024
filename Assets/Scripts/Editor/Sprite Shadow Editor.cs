using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Editor
{
    [CustomEditor(typeof(SpriteShadow))]
    public class SpriteShadowEditor : UnityEditor.Editor
    {
        public VisualTreeAsset visualTree;

        private SerializedProperty _prefabProperty;
        private SerializedProperty _visibilityProperty;
        private SerializedProperty _spriteNamesProperty;
        private SerializedProperty _maskProperty;
        private SerializedProperty _hasMaskProperty;

        private Foldout _visibilityFoldout;
        private Foldout _hasMaskFoldout;
        private ObjectField _prefabField;
        private PropertyField _maskField;

        private void OnEnable()
        {
            _prefabProperty = serializedObject.FindProperty("prefab");
            _visibilityProperty = serializedObject.FindProperty("visibility");
            _spriteNamesProperty = serializedObject.FindProperty("spriteNames");
            _maskProperty = serializedObject.FindProperty("mask");
            _hasMaskProperty = serializedObject.FindProperty("hasMask");
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            visualTree.CloneTree(root);

            _visibilityFoldout = root.Q<Foldout>("VisibilityFoldout");
            _hasMaskFoldout = root.Q<Foldout>("HasMaskFoldout");
            _prefabField = root.Q<ObjectField>("PrefabField");
            _maskField = root.Q<PropertyField>("MaskField");
            
            _prefabField.RegisterValueChangedCallback(_ => UpdateVisibilityFoldout());
            _maskField.RegisterValueChangeCallback(_ => UpdateMaskFoldout());

            return root;
        }

        private void UpdateVisibilityFoldout()
        {
            serializedObject.Update();

            _visibilityFoldout.contentContainer.Clear();

            if (_prefabProperty.objectReferenceValue != null)
            {
                _visibilityFoldout.style.display = DisplayStyle.Flex;

                for (var i = 0; i < _visibilityProperty.arraySize; i++)
                {
                    var index = i;
                    var spriteName = _spriteNamesProperty.GetArrayElementAtIndex(index).stringValue;
                    var toggle = new Toggle(spriteName)
                    {
                        value = _visibilityProperty.GetArrayElementAtIndex(index).boolValue
                    };

                    toggle.RegisterValueChangedCallback(evt =>
                    {
                        var visibilityElement = _visibilityProperty.GetArrayElementAtIndex(index);
                        visibilityElement.boolValue = evt.newValue;
                        serializedObject.ApplyModifiedProperties();
                    });

                    _visibilityFoldout.contentContainer.Add(toggle);
                }
            }
            else
            {
                _visibilityFoldout.style.display = DisplayStyle.None;
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        private void UpdateMaskFoldout()
        {
            serializedObject.Update();

            _hasMaskFoldout.contentContainer.Clear();

            if (_maskProperty.boolValue)
            {
                _hasMaskFoldout.style.display = DisplayStyle.Flex;

                for (var i = 0; i < _hasMaskProperty.arraySize; i++)
                {
                    var index = i;
                    var spriteName = _spriteNamesProperty.GetArrayElementAtIndex(index).stringValue;
                    var toggle = new Toggle(spriteName)
                    {
                        value = _hasMaskProperty.GetArrayElementAtIndex(index).boolValue
                    };

                    toggle.RegisterValueChangedCallback(evt =>
                    {
                        var hasMaskElement = _hasMaskProperty.GetArrayElementAtIndex(index);
                        hasMaskElement.boolValue = evt.newValue;
                        serializedObject.ApplyModifiedProperties();
                    });

                    _hasMaskFoldout.contentContainer.Add(toggle);
                }
            }
            else
            {
                _hasMaskFoldout.style.display = DisplayStyle.None;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
